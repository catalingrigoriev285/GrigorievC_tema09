**6.1. Utilizați pentru texturare imagini cu transparență și fără. Ce observați?**

La imaginile fără transparență: Textura este complet opacă. Imaginea acoperă integral suprafața obiectului și niciun element din spatele acestuia nu este vizibil prin textură.

La imaginile cu transparență: Zonele transparente ale imaginii fac ca porțiuni din obiect să devină invizibile sau semitransparente. Se poate vedea fundalul sau alte obiecte prin acele zone, deoarece texturile pot reține informații despre „canalele de transparență” și tuple de culoare RGBA, unde componenta alpha setează opacitatea.

**6.2. Ce formate de imagine pot fi aplicate în procesul de texturare în OpenGL?**

Deoarce OpenTK se bazează pe ```System.Drawing.Bitmap```, formatele suportate sunt : BMP, PNG, JPG/JPEG, GIF, TIFF, atâta timp cât pot fi decodificate într-un tablou de pixeli compatibil cu formatele OpenGL (RGB sau RGBA)

**6.3. Specificați ce se întâmplă atunci când se modifică culoarea (prin manipularea canalelor RGB) obiectului texturat.**

Atunci când se modifică culoarea unui obiect texturat, rezultatul vizual depinde de funcția de texturare aleasă pentru a calcula valoarea finală a pixelilor:

În cazul scalării - culoarea obiectului se combină cu cea a texturii.

În modul "blended" (amestecare) - culoarea definită este "amestecată" cu fragmentul texturat, modificând valoarea finală a fragmentului de textură.

În modurile "decal" sau "replace" - culoarea texturii este aplicată deasupra obiectului texturat, ceea ce înseamnă că modificarea culorii de bază a obiectului poate să nu fie vizibilă în zonele opace ale texturii.

Modificarea canalelor RGB are efectul cel mai vizibil în modul de modulație / scalare, unde culoarea obiectului interacționează direct cu imaginea texturii.

**6.4. Ce deosebiri există între scena ce utilizează obiecte texturate în modul iluminare activat, respectiv dezactivat?**

1. Iluminare DEZACTIVATĂ (GL_LIGHTING = Disabled)

Obiectul are un aspect "flat" și uniform. Textura este afișată la luminozitate maximă (sau la culoarea specificată prin glColor), indiferent de unghiul din care este privit sau de poziția sa în spațiu. De asemenea nu există umbre proprii pe suprafața obiectului. Fețele cubului, de exemplu, vor avea aceeași intensitate a texturii, chiar dacă unele ar trebui să fie "în umbră".

2. Iluminare ACTIVATĂ (GL_LIGHTING = Enabled)

Textura va apărea mai întunecată pe fețele care nu sunt orientate spre sursa de lumină și mai luminoasă pe cele expuse direct. Pot apărea zone de strălucire (specular highlights) peste textură, dacă materialul este setat să fie lucios.

Fără iluminare vedem imaginea fiind lipită pe obiect, iar cu iluminare, vedem un material care arată această imagine și reacționează la lumina din scenă.
