using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.IO;

namespace MachineSlot
{
    public partial class Form1 : Form
    {
        // variable de joc
        int credits, bet, win;

        // texturi
        private int[] textures = new int[3];
        private readonly string[] assetFiles = { "../../assets/cherry.jpg", "../../assets/red7.jpg", "../../assets/tenx.jpg" };

        private bool glReady;

        private readonly int[] reelSymbols = { 0, 1, 2 };
        private readonly float[] reelOffsets = { 0f, 0f, 0f };
        private readonly float[] reelSpeeds = { 0f, 0f, 0f };

        private readonly Random rng = new Random();

        private Timer animTimer;
        private bool spinning;
        private DateTime lastTick;
        private const float decel = 1.5f;

        // procentajul de castig
        private int winPercent = 100;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            glControl1.Load += GlControl1_Load;
            glControl1.Paint += glControl1_Paint;
            glControl1.Resize += glControl1_Resize;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            credits = 100;
            bet = 25;
            win = 0;

            label_credite.Text = credits.ToString();
            label_bet.Text = bet.ToString();
            label_win.Text = win.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl1.MakeCurrent();
            glControl1_Resize(this, EventArgs.Empty);
            GL.ClearColor(Color.DarkBlue);

            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            glControl1.VSync = true;

            TryInitTextures();

            animTimer = new Timer { Interval = 16 };
            animTimer.Tick += AnimTimer_Tick;
            animTimer.Start();

            lastTick = DateTime.UtcNow;

            glControl1.Invalidate();
        }

        private void GlControl1_Load(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();

            GL.ClearColor(Color.DarkBlue);

            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);

            glControl1.VSync = true;

            TryInitTextures();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();

            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            int w = glControl1.ClientSize.Width;
            int h = glControl1.ClientSize.Height;

            GL.Viewport(0, 0, w, h);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1, 1);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void button_more_Click(object sender, EventArgs e)
        {
            // crestere bet
            bet += 25;
            if (bet > credits) bet = credits;

            label_bet.Text = bet.ToString();
        }

        private void button_less_Click(object sender, EventArgs e)
        {
            // scadere bet
            bet -= 25;
            if (bet < 0) bet = 0;

            label_bet.Text = bet.ToString();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (!glReady)
            {
                TryInitTextures();
            }

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            DrawReels();

            glControl1.SwapBuffers();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //
        }

        // handler de timer, actualizeaza pozitia si viteza sloturilor la fiecare tick
        /* Functia este apelata la fiecare frame al animatiei si actualizeaza rotirea rotilor,
         * 1. Calculeaza timpul scurs de la ultimul frame (dt)
         * 2. Pentru fiecare slot care se invarte:
         * Actualizeaza pozitia, normalizeaza offset-ul intre 0,1, scade viteza treptat
         * 3. Daca toate sloturile s-au oprit, se evalueaza castigul */
        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            // calculam timpul ramas (dt) dintre cadre
            var now = DateTime.UtcNow;
            float dt = (float)(now - lastTick).TotalSeconds;
            if (dt <= 0f || dt > 0.25f) dt = 1f / 60f; // daca dt e prea mic sau prea mare se forteaza la 60 cadre pe secunda
            lastTick = now;
            // se calculeaza cate secunde au trecut de la ultimul frame

            bool anySpinning = false;

            // parcurgem fiecare slot
            for (int i = 0; i < 3; i++)
            {
                // daca slotul se invarte
                if (Math.Abs(reelSpeeds[i]) > 0.0001f)
                {
                    anySpinning = true;

                    // actualizam offsetul slotului
                    reelOffsets[i] += reelSpeeds[i] * dt;

                    // normalizam intre [0,1)
                    if (reelOffsets[i] >= 1f) reelOffsets[i] -= 1f;
                    if (reelOffsets[i] < 0f) reelOffsets[i] += 1f;

                    // deacceleram
                    reelSpeeds[i] = Math.Max(0f, reelSpeeds[i] - decel * dt);

                    // daca viteza slotului devine prea mic, slotul se opreste brusc si resetam offsetul
                    if (reelSpeeds[i] <= 0.0005f)
                    {
                        reelSpeeds[i] = 0f;
                        reelOffsets[i] = 0f;
                        reelSymbols[i] = rng.Next(0, textures.Length);
                    }
                }
            }

            // cand toate sloturile s-au oprit
            if (!anySpinning && spinning)
            {
                EvaluateWin();
                spinning = false;
            }

            // desenam animatia
            if (anySpinning)
            {
                glControl1.Invalidate();
            }
        }

        // functie pentru calculul castigului
        private void EvaluateWin()
        {
            // daca toate sloturile au acelasi simbol
            if (reelSymbols[0] == reelSymbols[1] && reelSymbols[1] == reelSymbols[2])
            {
                int symbol = reelSymbols[0];

                // multiplicator in functie de simbol, 2 -> x10, 1 -> x7, altul -> x2
                int multiplier = symbol == 2 ? 10 : (symbol == 1 ? 7 : 2);

                // ex. bet = 10, multiplicator = 7, baseWin = 70
                int baseWin = bet * multiplier;

                // variabla care stocheaza procentul de castig, fiind limitat intre 0,100
                int w = (int)Math.Round(baseWin * Math.Max(0, Math.Min(100, winPercent)) / 100.0);

                // aplicam castigul
                credits += w;
                win = w;
            }
            else
            {
                // daca nu, setam win pe 0
                win = 0;
            }

            label_win.Text = win.ToString();
            label_credite.Text = credits.ToString();
        }

        private void DrawReels()
        {
            int w = glControl1.ClientSize.Width;
            int h = glControl1.ClientSize.Height;

            int reelWidth = Math.Max(64, w / 5);
            int reelHeight = Math.Max(64, (int)(h * 0.6));

            int gap = Math.Max(reelWidth / 3, w / 30);

            int totalWidth = 3 * reelWidth + 2 * gap;

            int startX = (w - totalWidth) / 2; // centram pe orizontal
            int y = (h - reelHeight) / 2; // centram pe vertical

            for (int i = 0; i < 3; i++)
            {
                int x = startX + i * (reelWidth + gap);

                DrawReel(x, y, reelWidth, reelHeight, reelSymbols[i], reelOffsets[i]);

                // separator
                if (i < 2)
                {
                    int sepX = x + reelWidth + (gap / 2) - 2;
                    DrawSeparator(sepX, y, 4, reelHeight);
                }
            }
        }

        private void DrawReel(int x, int y, int width, int height, int symbolIndex, float offset)
        {
            DrawReelBackground(x, y, width, height);

            int spacer = Math.Max(2, height / 12); // spatiu dintre simboluri
            int symH = height - spacer; // intaltimea simbolului
            int step = symH + spacer; // distanta dintre simboluri

            int baseY = y - (int)(offset * step); // pozitia simbolului fata de offset ( animatie )

            // margini interioare
            int insetX = Math.Max(2, width / 20);
            int drawW = width - insetX * 2;

            // texturile consecutive de desenare
            int tex0 = textures.Length > 0 ? textures[symbolIndex % textures.Length] : 0;
            int tex1 = textures.Length > 0 ? textures[(symbolIndex + 1) % textures.Length] : 0;

            int bezel = Math.Max(2, width / 30);
            int clipW = width - 2 * bezel;
            int windowH = Math.Max(24, (int)(height));
            int clipX = x + bezel;
            int clipY = y + (height - windowH) / 2;

            GL.Enable(EnableCap.ScissorTest);
            int scX = clipX;

            // scY pentru ca scissor foloseste coordonate de jos in sus
            int scY = glControl1.ClientSize.Height - (clipY + windowH);
            GL.Scissor(scX, scY, clipW, windowH);

            // desenam cele 2 texturi
            GL.BindTexture(TextureTarget.Texture2D, tex0);
            DrawTexturedQuad(x + insetX, baseY, drawW, symH);

            GL.BindTexture(TextureTarget.Texture2D, tex1);
            DrawTexturedQuad(x + insetX, baseY + step, drawW, symH);

            // dezactivam clipping-ul
            GL.Disable(EnableCap.ScissorTest);
        }

        private void DrawTexturedQuad(int x, int y, int width, int height)
        {
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0); GL.Vertex2(x, y);
            GL.TexCoord2(1, 0); GL.Vertex2(x + width, y);
            GL.TexCoord2(1, 1); GL.Vertex2(x + width, y + height);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y + height);

            GL.End();
        }

        private void DrawReelBackground(int x, int y, int width, int height)
        {
            GL.Disable(EnableCap.Texture2D);

            GL.Color3(0.9f, 0.9f, 0.9f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(x + width, y);
            GL.Vertex2(x + width, y + height);
            GL.Vertex2(x, y + height);
            GL.End();

            int bezel = Math.Max(2, width / 30);

            GL.Color3(0.1f, 0.1f, 0.1f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x + bezel, y + bezel);
            GL.Vertex2(x + width - bezel, y + bezel);
            GL.Vertex2(x + width - bezel, y + height - bezel);
            GL.Vertex2(x + bezel, y + height - bezel);
            GL.End();

            GL.Color3(1f, 1f, 1f);
            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawSeparator(int x, int y, int width, int height)
        {
            GL.Disable(EnableCap.Texture2D);

            GL.Color3(0.2f, 0.2f, 0.2f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(x + width, y);
            GL.Vertex2(x + width, y + height);
            GL.Vertex2(x, y + height);
            GL.End();

            GL.Color3(1f, 1f, 1f);
            GL.Enable(EnableCap.Texture2D);
        }

        private void TryInitTextures()
        {
            if (glReady) return;
            glControl1.MakeCurrent();

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = 0;
            }

            for (int i = 0; i < assetFiles.Length && i < textures.Length; i++)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assetFiles[i]);
                if (!File.Exists(path))
                {
                    string alt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", assetFiles[i]);
                    if (File.Exists(alt)) path = Path.GetFullPath(alt);
                }

                textures[i] = LoadTextureFromFile(path);
            }

            glReady = true;
        }

        private void button_add_credits_Click(object sender, EventArgs e)
        {
            this.credits += 100;
            label_credite.Text = credits.ToString();
        }

        private int LoadTextureFromFile(string file)
        {
            // daca fisierul exista
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return 0;

            // initializam un noi id de textura si il activam
            int texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            using (var bmp = new Bitmap(file))
            {
                using (var bmpData = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                using (var g = Graphics.FromImage(bmpData))
                {
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                    var data = bmpData.LockBits(new Rectangle(0, 0, bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                    bmpData.UnlockBits(data);
                }
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            // returnam id-ul texturii
            return texId;
        }

        private void button_spin_Click_1(object sender, EventArgs e)
        {
            if (bet <= 0 || bet > credits) return;

            credits -= bet;
            label_credite.Text = credits.ToString();
            win = 0; label_win.Text = "0";

            // setam viteze initiale pentru cele 3 sloturi
            reelSpeeds[0] = 2.5f + (float)rng.NextDouble() * 1.5f;
            reelSpeeds[1] = 2.5f + (float)rng.NextDouble() * 1.5f;
            reelSpeeds[2] = 2.5f + (float)rng.NextDouble() * 1.5f;

            spinning = true; // setam flag-ul de rotire

            // resetam timpul de referinta pentru animatie
            lastTick = DateTime.UtcNow;

            glControl1.Invalidate();
        }
    }
}