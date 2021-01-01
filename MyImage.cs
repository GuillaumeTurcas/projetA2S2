using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LectureImage
{
    class MyImage
    {
        private int hauteur;
        private int taille;
        private int largeur;
        private string type;
        private Pixel[,] imageInit;
        private int bit;

        public MyImage(string fichier)
        {
            byte[] myfile = File.ReadAllBytes(fichier);

            int a = myfile[0]; int b = myfile[1]; //Type
            if (a == 66 && b == 77) { type = ".bmp"; }
            else { type = "autre"; }

            taille = myfile[2] + myfile[3] * 256 + myfile[4] * 65536 + myfile[5] * 16777216; //conversion directe du little endian vers un entier

            largeur = myfile[18] + myfile[19] * 256 + myfile[20] * 65536 + myfile[21] * 16777216;

            hauteur = myfile[22] + myfile[23] * 256 + myfile[24] * 65536 + myfile[25] * 16777216;

            bit = myfile[28] + myfile[29] * 256;

            imageInit = new Pixel[hauteur, largeur];
            int k = 54;
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)
                {
                    int[] pixelprov = { myfile[k], myfile[k + 1], myfile[k + 2] };
                    imageInit[i, j] = new Pixel(pixelprov);
                    k = k + 3;
                }
            }
        }

        public int Taille { get { return taille; } }

        public int Largeur { get { return largeur; } }

        public int Hauteur { get { return hauteur; } }

        public int Bit { get { return bit; } }

        public string Type { get { return type; } }

        /// <summary>
        /// Conversion en litlle endian
        /// </summary>
        /// <param name="taille"></param>
        /// <returns></returns>

        static byte[] IntToLitEndian(int taille)
        {
            byte[] endian = new byte[4]; //on suppose qu'il est initialisé à 0
            if (taille < Math.Pow(2, 32))
            {
                int d = Math.DivRem(taille, (int)(Math.Pow(2, 24)), out int r);
                int c = Math.DivRem(r, (int)(Math.Pow(2, 16)), out int r2);
                int b = Math.DivRem(r2, (int)(Math.Pow(2, 8)), out int a);
                //nos 4 entiers sont inférieurs à 256 donc ont les convertit en byte.
                byte poidsFort = (byte)d;
                byte deuxieme = (byte)c;
                byte troisieme = (byte)b;
                byte poidsFaible = (byte)a;
                endian[0] = poidsFaible; endian[1] = troisieme; endian[2] = deuxieme; endian[3] = poidsFort;
            }
            else Console.WriteLine("ERROR : file size is too big");
            return endian;
        }

        public Pixel[,] Image { get { return imageInit; } }

        /// <summary>
        /// applique un filtre noir et blanc
        /// </summary>
        /// <returns></returns>

        public Pixel[,] NoirEtBlanc()
        {
            Pixel[,] imageNB = new Pixel[hauteur, largeur];
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)
                {
                    int nivGris = (imageInit[i, j].Pixels[0] + imageInit[i, j].Pixels[1] + imageInit[i, j].Pixels[2]) / 3; //moyenne de R, V et B
                    int[] pixelgris = new int[3]; pixelgris[0] = nivGris; pixelgris[1] = nivGris; pixelgris[2] = nivGris;
                    Pixel pixelgrispix = new Pixel(pixelgris);
                    imageNB[i, j] = pixelgrispix;
                }
            }
            return imageNB;
        }

        public Pixel[,] Negatif()
        {
            Pixel[,] imageNeg = new Pixel[hauteur, largeur];
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)
                {
                    int pixelR = 255 - imageInit[i, j].Pixels[0];
                    int pixelV = 255 - imageInit[i, j].Pixels[1];
                    int pixelB = 255 - imageInit[i, j].Pixels[2];
                    int[] newPixel = { pixelR, pixelV, pixelB };
                    imageNeg[i, j] = new Pixel(newPixel);
                }
            }
            return imageNeg;
        }

        /// <summary>
        /// permet la rotation de l'image
        /// </summary>
        /// <param name="demande"></param>
        /// <returns></returns>

        public Pixel[,] Rotation(int demande)
        {
            Pixel[,] imageRot = null;
            if (demande == 270)
            {
                imageRot = new Pixel[largeur, hauteur];
                for (int i = 0; i < imageInit.GetLength(1); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(0); j++)
                    {
                        imageRot[i, j] = imageInit[hauteur - 1 - j, i];
                    }
                }
            }

            if (demande == 180)
            {
                imageRot = new Pixel[hauteur, largeur];
                for (int i = 0; i < imageInit.GetLength(0); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(1); j++)
                    {
                        imageRot[i, j] = imageInit[imageInit.GetLength(0) - 1 - i, imageInit.GetLength(1) - 1 - j];
                    }
                }
            }

            if (demande == 90)
            {
                imageRot = new Pixel[largeur, hauteur];
                for (int i = 0; i < imageInit.GetLength(1); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(0); j++)
                    {
                        imageRot[i, j] = imageInit[j, largeur - 1 - i];
                    }
                }
            }
            return imageRot;
        }

        /// <summary>
        /// permet d'appliquer un mode miroir à limage
        /// </summary>
        /// <param name="demande"></param>
        /// <returns></returns>

        public Pixel[,] Miroir(char demande)
        {
            Pixel[,] imageMir = new Pixel[hauteur, largeur];
            if (demande == 'h')  //miroir horizontal
            {
                for (int i = 0; i < imageInit.GetLength(0); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(1); j++)
                    {
                        imageMir[i, j] = imageInit[imageInit.GetLength(0) - i - 1, j];
                    }
                }
            }
            if (demande == 'v')  // miroir vertical
            {
                for (int i = 0; i < imageInit.GetLength(0); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(1); j++)
                    {
                        imageMir[i, j] = imageInit[i, imageInit.GetLength(1) - j - 1];
                    }
                }
            }
            return imageMir;
        }

        /// <summary>
        /// pour redimensionner l'image
        /// </summary>
        /// <param name="redim"></param>
        /// <returns></returns>

        public Pixel[,] Redim(int redim)
        {
            Pixel[,] imageRedim = null;
            if (redim == 2)  //aggrandissement
            {
                imageRedim = new Pixel[hauteur * 2, largeur * 2];
                for (int i = 0; i < imageInit.GetLength(0); i++)
                {
                    for (int j = 0; j < imageInit.GetLength(1); j++)
                    {
                        imageRedim[i * 2, j * 2] = imageRedim[1 + i * 2, j * 2] = imageRedim[i * 2, 1 + j * 2] = imageRedim[1 + i * 2, 1 + j * 2] = imageInit[i, j];
                    }
                }
            }
            if (redim == 3) //reduction
            {
                int newHauteur = hauteur / 2; int newLargeur = largeur / 2;
                imageRedim = new Pixel[newHauteur, newLargeur];
                for (int i = 0; i < imageRedim.GetLength(0); i++)
                {
                    for (int j = 0; j < imageRedim.GetLength(1); j++)
                    {
                        imageRedim[i, j] = imageInit[i * 2, j * 2];
                    }
                }
            }
            if (imageRedim == null)
            {
                Console.WriteLine("Impossible de redimensionner l'image, car les conditions demandées ne sont pas respectées");
            }
            return imageRedim;
        }

        /// <summary>
        /// multiplication de deux matrices
        /// </summary>
        /// <param name="matrice1"></param>
        /// <param name="matrice2"></param>
        /// <returns></returns>

        public int[,] MultMat3p3(int[,] matrice1, int[,] matrice2)
        {
            int[,] matreturn = null;
            if (matrice1.GetLength(0) == 3 && matrice1.GetLength(1) == 3 && matrice2.GetLength(0) == 3 && matrice2.GetLength(1) == 3)
            {
                matreturn = new int[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        matreturn[i, j] = 0;
                        for (int m = 0; m < 3; m++)
                        {
                            matreturn[i, j] += matrice1[i, m] * matrice2[m, j];
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Dimensions de matrice incorrecte");
            }
            return matreturn;
        }

        /// <summary>
        /// Conversion en pixel
        /// </summary> sert pour la matrice de convolution
        /// <param name="matriceConv"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="decal"></param>
        /// <param name="div"></param>
        /// <returns></returns>

        public Pixel PixelToConv(int[,] matriceConv, int i, int j, int decal, int div)
        {

            int[,] matriceCalculR = new int[3, 3]; int[,] matriceCalculG = new int[3, 3];
            int[,] matriceCalculB = new int[3, 3];


            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                    if (l == -1 || k == -1 || l == largeur || k == hauteur) //on ne prend pas en compte car en dehors des limites de l'image
                    {
                        matriceCalculR[k - i + 1, l - j + 1] = 0; //de même pour le vert et le bleu, la matrice est initialisée à zero en c#
                    }
                    else
                    {
                        matriceCalculR[k - i + 1, l - j + 1] = imageInit[k, l].Pixels[0];
                        matriceCalculG[k - i + 1, l - j + 1] = imageInit[k, l].Pixels[1];
                        matriceCalculB[k - i + 1, l - j + 1] = imageInit[k, l].Pixels[2];
                    }
                }
            }

            int[,] matriceResultR = MultMat3p3(matriceCalculR, matriceConv);
            int[,] matriceResultG = MultMat3p3(matriceCalculG, matriceConv);
            int[,] matriceResultB = MultMat3p3(matriceCalculB, matriceConv);

            int[] tabPixelToMatrice = { decal + matriceResultR[1, 1] / div, decal + matriceResultG[1, 1] / div, decal + matriceResultB[1, 1] / div };
            Pixel pixelToMatrice = new Pixel(tabPixelToMatrice);

            return pixelToMatrice;
        }

        /// <summary>
        /// Création de fractale de Mandelbrot
        /// </summary>
        /// <returns></returns>

        public Pixel[,] Fractal()
        {
            double x1 = -2.1; double x2 = 0.6;
            double y1 = -1.2; double y2 = 1.2; //zone du plan délimitant la fractale de mandelbrot
            int zoom = 1000; int itteration_Max = 50; //paramère qui donne la précision
            int hauteur = (int)((x2 - x1) * (double)zoom); int largeur = (int)((y2 - y1) * zoom);//calcule la taille (en pixel)
            Pixel[,] image2 = new Pixel[largeur, hauteur];

            for (int x = 0; x < hauteur; x++)
            {
                for (int y = 0; y < largeur; y++)
                {
                    /*On étudie la convergence de la suite z(n+1) = (z(n))² + c
                     * Z = (zr+ j zi)² + cr + j ci où j est la racine carrée de -1
                     * Z = zr² - zi² + cr + j * (2*zi*zr + ci)     On sépare notre suite en suite réelle d'une part et suite imaginaire d'autre part
                     */
                    double c_r = (double)x / zoom + x1; //coordonées du point de calcul
                    double c_i = (double)y / zoom + y1;
                    double z_r = 0; double z_i = 0; // valeurs initiales de la suite
                    int i = 0;
                    do
                    {
                        double tmp = z_r;
                        z_r = z_r * z_r - z_i * z_i + c_r; //terme suivant de la suite réelle 
                        z_i = 2 * z_i * tmp + c_i; //terme suivant de la suite imaginaire
                        i = i + 1;
                    } while (z_r * z_r + z_i * z_i < 4 && i < itteration_Max); //si le module est supérieur à 2 (le carré du module supérieur à 4) alors la suite diverge

                    if (i == itteration_Max) //on considère qu'il y a convergence
                    {
                        int[] petitPixel = { 0, 0, 0 }; //noir
                        image2[y, x] = new Pixel(petitPixel);
                    }
                    else
                    {
                        int[] autrePixel = { 255, 255, 255 }; //blanc
                        image2[y, x] = new Pixel(autrePixel);
                    }

                }
            }

            return image2;
        }

        /// <summary>
        /// Fonction Avancée pour la matrice de convolution
        /// </summary>
        /// <param name="matriceConv"></param>
        /// <param name="decal"></param>
        /// <param name="div"></param>
        /// <returns></returns>

        public Pixel[,] FonctionAvance(int[,] matriceConv, int decal, int div)
        {
            Pixel[,] matriceAvance = new Pixel[hauteur, largeur];

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = -0; j < largeur; j++)
                {
                    matriceAvance[i, j] = PixelToConv(matriceConv, i, j, decal, div);
                }
            }

            return matriceAvance;
        }

        /// <summary>
        /// Histogram
        /// </summary>
        /// <returns></returns>

        public Pixel[,] Histogram()
        {
            int[] histotab = new int[768];
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (k == 0)
                        {
                            int r = imageInit[i, j].Pixels[0]; //rouge
                            histotab[r]++;
                        }
                        if (k == 1)
                        {
                            int v = imageInit[i, j].Pixels[1] + 256; //vert
                            histotab[v]++;
                        }
                        if (k == 2)
                        {
                            int b = imageInit[i, j].Pixels[2] + 512; //bleu
                            histotab[b]++;
                        }
                    }
                }
            }
            int max = 0;
            for (int i = 0; i < histotab.Length; i++)
            {
                int maxprov = histotab[i];
                if (maxprov > max) max = maxprov; //on dimensionne l'histogrmme en fonction de la plus grande valeur
            }
            max += 4 - (max % 4); //on ne trvaille qu'avec des images multiples de 4 car redimensionnables
            Pixel[,] histo = new Pixel[768, max];
            for (int i = 0; i < 768; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    int[] blanc = { 255, 255, 255 };
                    int[] noir = { 0, 0, 0 };
                    if (j < histotab[i]) histo[i, j] = new Pixel(noir);
                    else histo[i, j] = new Pixel(blanc);
                    //Console.WriteLine(histo[i, j].Pixels[1]);
                }
            }
            return histo;
        }

        /// <summary>
        /// Code une image dans une autre
        /// </summary>
        /// <param name="image2"></param>
        /// <returns></returns>

        public Pixel[,] ImageImage(Pixel[,] image2)
        {
            //avoir accès à une deuxième image (taille2 ,hauteur2, largeur2 , image2 sa matrice de pixels)
            //il faut que la première image soit plus grande ou égale en haureur et largeur sinon on perd de l'information
            Pixel[,] NouvelleImage = new Pixel[hauteur, largeur];
            //optimisation de la mémoire: déclaration des variables pour les réutiliser
            int R1 = 0;
            int R2 = 0;
            int G1 = 0;
            int G2 = 0;
            int B1 = 0;
            int B2 = 0;
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)

                {
                    R1 = imageInit[i, j].Pixels[0] / 16;
                    G1 = imageInit[i, j].Pixels[1] / 16;
                    B1 = imageInit[i, j].Pixels[2] / 16;
                    R1 *= 16;
                    G1 *= 16;
                    B1 *= 16;
                    if (i < image2.GetLength(0) && j < image2.GetLength(1))
                    {
                        R2 = image2[i, j].Pixels[0] / 16;
                        G2 = image2[i, j].Pixels[1] / 16;
                        B2 = image2[i, j].Pixels[2] / 16;

                        int[] newPixel = { R1 + R2, G1 + G2, B1 + B2 };
                        NouvelleImage[i, j] = new Pixel(newPixel);
                    }
                    else
                    {
                        int[] newPixel = { R1, G1, B1 }; //cas où l'on sort de l'image 2
                        NouvelleImage[i, j] = new Pixel(newPixel);
                    }

                }
            }
            return NouvelleImage;
        }
        /// <summary>
        /// Décode une image secrète si cachée par la méthode ImageImage
        /// </summary>
        /// <returns></returns>
        public Pixel[,] TrouveImageCachee()
        {
            Pixel[,] ImageCachee = new Pixel[hauteur, largeur];
            for (int i = 0; i < imageInit.GetLength(0); i++)
            {
                for (int j = 0; j < imageInit.GetLength(1); j++)
                {
                    int Roriginal = Math.DivRem(imageInit[i, j].Pixels[0], 16, out int Rcache);//division euclidienne
                    int Goriginal = Math.DivRem(imageInit[i, j].Pixels[1], 16, out int Gcache);
                    int Boriginal = Math.DivRem(imageInit[i, j].Pixels[2], 16, out int Bcache);
                    //on ne s'intéresse pas à la première image car elle n'a pas beaucoup changé pendant l'étape de cryptage événtuelle
                    //les variables R,G,B originales ne seront pas utilisées
                    Rcache *= 16;
                    Gcache *= 16;
                    Bcache *= 16;
                    int[] newPixel = { Rcache, Gcache, Bcache };
                    ImageCachee[i, j] = new Pixel(newPixel);
                }
            }
            return ImageCachee;
        }
        /// <summary>
        /// Afficher Image
        /// </summary>
        /// <param name="imageAffich"></param>
        /// <param name="fichier"></param>


        public void AfficherImage(Pixel[,] imageAffich, string fichier)
        {
            int newHauteur = imageAffich.GetLength(0); int newLargeur = imageAffich.GetLength(1); int avanceMyfile = 0;

            int newTaille = newHauteur * newLargeur * 3 + 54;
            byte[] myfileImageInit = File.ReadAllBytes(fichier);
            byte[] myfileImageAffich = new byte[newTaille];
            for (int i = 0; i < 54; i++) { myfileImageAffich[i] = myfileImageInit[i]; }// voir ~

            for (int i = 2; i < 6; i++) { myfileImageInit[i] = IntToLitEndian(newTaille)[avanceMyfile]; avanceMyfile++; } //taille

            avanceMyfile = 0;
            for (int k = 18; k < 22; k++)
            {
                myfileImageAffich[k] = IntToLitEndian(newLargeur)[avanceMyfile]; avanceMyfile++;//largeur
            }
            avanceMyfile = 0;
            for (int k = 22; k < 26; k++)
            {
                myfileImageAffich[k] = IntToLitEndian(newHauteur)[avanceMyfile]; avanceMyfile++;//hauteur
            }
            // ~ on a gardé le type d'image original, la taille du header (54) et le nombre de bit de l'image originale
            avanceMyfile = 54;
            for (int i = 0; i < newHauteur; i++)
            {
                for (int j = 0; j < newLargeur; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte bytepixel = (byte)imageAffich[i, j].Pixels[k];
                        myfileImageAffich[avanceMyfile] = bytepixel; avanceMyfile++;
                    }
                }
            }

            File.WriteAllBytes("ImageDeSortie.bmp", myfileImageAffich);
            Process.Start("ImageDeSortie.bmp");
        }
    }
}