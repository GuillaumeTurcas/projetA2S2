using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LectureImage
{
    class Program
    {
        public static int SaisieNombre() //permet de demander un entier
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            { }
            return result;
        }

        static string ImageUtilis() //permet de sélectionner parmi les images disponibles
        {
            Console.WriteLine("\nQuelle image voulez-vous utiliser :\n\n"
                                 + "(1) : Test.bmp \n"
                                 + "(2) : coco.bmp \n"
                                 + "(3) : lac_en_montagne.bmp\n"
                                 + "(4) : lena.bmp\n"
                                 + "(0) : Image Précédente (qui a déjà reçu des opérations)\n\n"
                                 + "Sélectionnez le numéro pour la commande désirée \n");

            string fichier = "coco.bmp";
            int choix = SaisieNombre();
            if (choix == 1) fichier = "Test.bmp";
            if (choix == 2) fichier = "coco.bmp";
            if (choix == 3) fichier = "lac_en_montagne.bmp";
            if (choix == 4) fichier = "lena.bmp";
            if (choix == 0) fichier = "ImageDeSortie.bmp";
            Console.Clear(); return fichier;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n"
                            + "     Image Creator\n"
                            + "     par\n"
                            + "     Guillaume Turcas et\n"
                            + "     Victor Lenoble\n");

            Console.ReadKey(); Console.Clear();
            string fichier = ImageUtilis();
            MyImage image = new MyImage(fichier);

            ConsoleKeyInfo cki;
            do
            {
                image = new MyImage(fichier);
                Console.Clear();
                Console.WriteLine("\nMenu :\n\n"
                                 + "(1) : Afficher l'image \n"
                                 + "(2) : Donner les métadonnées de l'image \n"
                                 + "(3) : Fonctions Simples\n"
                                 + "(4) : Fonctions Avancées\n\n"
                                 + "(5) : Afficher l'Histogramme de l'image\n"
                                 + "(6) : Création d'une fractale\n"
                                 + "(7) : Image dans l'image\n"
                                 + "(8) : Décrypter une image cachée\n\n"
                                 + "(9) : Ouvrir le Rapport\n"
                                 + "(0) : Changer d'image \n\n"
                                 + "Sélectionnez le numéro pour la commande désirée \n");
                int num = SaisieNombre();
                Console.Clear();
                switch (num)
                {
                    case 0:
                        fichier = ImageUtilis();
                        image = new MyImage(fichier);
                        break;
                    case 1:
                        Console.WriteLine("\nVous avez selectionné le numéro 1 : Afficher l'image");
                        Console.ReadKey();
                        image.AfficherImage(image.Image, fichier);
                        break;

                    case 2:
                        Console.WriteLine("\nVous avez selectionné le numéro 2 : Donner les métadonnées de l'image");
                        Console.ReadKey();
                        Console.WriteLine(fichier + " :\n\n"
                                        + "Taille : " + image.Taille + " octets\n"
                                        + "Image codée en : " + image.Bit + " bits\n"
                                        + "Hauteur : " + image.Hauteur + " pixels\n"
                                        + "Largeur : " + image.Largeur + " pixels\n"
                                        + "Extention de l'image : " + image.Type
                                        + "\n");
                        break;

                    case 3:
                        Console.WriteLine("\nVous avez selectionné le numéro 3 : Fonctionalités simples");
                        Console.ReadKey();
                        Console.WriteLine("Menu :\n"
                                        + "(1) : Passage en Noir et Blanc\n"
                                        + "(2) : Faire une rotation de l'image\n"
                                        + "(3) : Appliquer le mode Miroir\n"
                                        + "(4) : Redimensionner l'image\n"
                                        + "(5) : Passage en Négatif\n" //Création personalisée
                                        + "(0) : Retour au menu précédent\n"
                                        + "\n"
                                        + "Sélectionnez le numéro pour la commande désirée \n");
                        int num0 = SaisieNombre();
                        switch (num0)
                        {
                            case 1:
                                Console.WriteLine("\nVous avez selectionné le numéro 1 : Passage en Noir et Blanc");
                                image.AfficherImage(image.NoirEtBlanc(), fichier);
                                break;
                            case 2:
                                Console.WriteLine("\nVous avez selectionné le numéro 2 : Faire une rotation de l'image");
                                Console.ReadKey();
                                int rot = 0;
                                while (rot != 90 && rot != 180 && rot != 270)
                                {
                                    Console.Clear();
                                    Console.WriteLine(fichier + " :\nQuel type de rotation-voulez-vous effectuer ? (90, 180, 270)\n");
                                    rot = SaisieNombre();
                                }
                                Console.Clear();
                                image.AfficherImage(image.Rotation(rot), fichier);
                                break;
                            case 3:
                                Console.WriteLine("\nVous avez selectionné le numéro 3 : Activer le mode Miroir");
                                Console.ReadKey();
                                char mir = 'r';
                                while (mir != 'h' && mir != 'v')
                                {
                                    Console.Clear();
                                    Console.WriteLine(fichier + " :\nQuel type de mode Miroir voulez-vous effectuer :\n"
                                                              + "(h) : Mode Miroir horizontal\n" + "(v) : Mode Miroir vertical");
                                    bool lettre = char.TryParse(Console.ReadLine(), out mir);
                                    if (!lettre) mir = 'r';
                                }
                                Console.Clear();
                                image.AfficherImage(image.Miroir(mir), fichier);
                                break;
                            case 4:
                                Console.WriteLine("\nVous avez selectionné le numéro 6 : Redimensionner l'image");
                                Console.ReadKey();
                                int dim = -1;
                                while (dim != 1 && dim != 2 && dim != 3)
                                {
                                    Console.Clear();
                                    Console.WriteLine(fichier + " :\nQuel type de redimension voulez-vous effectuer :\n"
                                                              + "(1) : Ne Rien faire\n" + "(2) : Agrandissement \n" + "(3) : Rétrécissement");
                                    dim = SaisieNombre();
                                }
                                Console.Clear();
                                if (dim != 1) image.AfficherImage(image.Redim(dim), fichier);
                                else image.AfficherImage(image.Image, fichier);
                                break;
                            case 5:
                                Console.WriteLine("\nVous avez selectionné le numéro 5 : Passage en Négatif");
                                image.AfficherImage(image.Negatif(), fichier);
                                break;
                            default: break;

                        }
                        break;

                    case 4:
                        Console.WriteLine("Menu des fonctions Avancées:\n"
                                         + "(1) : Application du repoussage\n"
                                         + "(2) : Application d'un flou \n"
                                         + "(3) : Détéctions des bords\n"
                                         + "(4) : Renforcement des bords\n"
                                         + "(5) : Création de filtre personalisé [PRO]\n" //Création personalisée
                                         + "(0) : Retour au menu précédent\n"
                                         + "Sélectionnez le numéro pour la commande désirée \n");
                        int num1 = SaisieNombre();
                        Console.Clear();
                        int[,] matriceConv = new int[3, 3]; //matrice de convolution pour les filtres
                        int div = 1;
                        int decal = 0;
                        switch (num1)
                        {
                            case 1:
                                Console.WriteLine("\nVous avez selectionné le numéro 1 : Application du Repoussage");
                                matriceConv[0, 0] = -2; matriceConv[2, 2] = 2; matriceConv[1, 0] = -1;
                                matriceConv[0, 1] = -1; matriceConv[1, 1] = 0; matriceConv[2, 1] = 1; matriceConv[1, 2] = 1;
                                Console.WriteLine("\nQuel diviseur et quel décalage voulez-vous effectuer ?");
                                Console.WriteLine("Diviseur: ");
                                div = SaisieNombre();
                                Console.WriteLine("Décalage: ");
                                decal = SaisieNombre();
                                break;
                            case 2:
                                Console.WriteLine("\nVous avez selectionné le numéro 2 : Application d'un flou");
                                for (int i = 0; i < 3; i++) { for (int j = 0; j < 3; j++) { matriceConv[i, j] = 1; } }
                                div = 3;
                                break;
                            case 3:
                                Console.WriteLine("\nVous avez selectionné le numéro 3 : Détection des bords");
                                matriceConv[0, 1] = -1; matriceConv[1, 1] = 1;
                                div = 5; decal = 1;
                                break;
                            case 4:
                                Console.WriteLine("\nVous avez selectionné le numéro 4 : Renforcement des bords");
                                matriceConv[1, 0] = 1; matriceConv[0, 1] = 1; matriceConv[1, 1] = -4; matriceConv[2, 1] = 1; matriceConv[1, 2] = 1;
                                Console.WriteLine("Diviseur: ");
                                div = SaisieNombre();
                                Console.WriteLine("Décalage: ");
                                decal = SaisieNombre();
                                break;
                            case 5:
                                Console.ReadKey();
                                for (int i = 0; i < 3; i++)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nVous avez selectionné le numéro 5 : Création de filtre personalisé");
                                        Console.WriteLine("\nCréez votre matrice:\n");
                                        AfficherMatrice(matriceConv);
                                        matriceConv[i, j] = SaisieNombre();
                                    }
                                }
                                break;
                            default: break;
                        }
                        if (num1 != 0) image.AfficherImage(image.FonctionAvance(matriceConv, decal, div), fichier);
                        break;
                    case 5:
                        Console.WriteLine("\nVous avez selectionné le numéro 5 : Afficher l'histogramme de l'image");
                        Console.ReadKey();
                        image.AfficherImage(image.Histogram(), fichier);
                        break;
                    case 6:
                        Console.WriteLine("\nVous avez selectionné le numéro 6 : Création d'une fractale");
                        Console.ReadKey();
                        image.AfficherImage(image.Fractal(), fichier);
                        break;
                    case 7:
                        Console.WriteLine("\nAvec quelle image voulez-vous fusionner l'image + " + fichier + " :\n\n"
                                 + "(1) : Test.bmp \n"
                                 + "(2) : coco.bmp \n"
                                 + "(3) : lac_en_montagne.bmp\n"
                                 + "(4) : lena.bmp\n"
                                 + "(0) : Image Précédente (qui a déjà reçu des opérations)\n\n"
                                 + "Sélectionnez le numéro pour la commande désirée \n");

                        string fichier2 = "coco.bmp";
                        int choix = SaisieNombre();
                        if (choix == 1) fichier2 = "Test.bmp";
                        if (choix == 2) fichier2 = "coco.bmp";
                        if (choix == 3) fichier2 = "lac_en_montagne.bmp";
                        if (choix == 4) fichier2 = "lena.bmp";
                        if (choix == 0) fichier2 = "ImageDeSortie.bmp";
                        MyImage image2 = new MyImage(fichier2);
                        Console.ReadKey();
                        image.AfficherImage(image.ImageImage(image2.Image), fichier);
                        break;
                    case 8:
                        Console.WriteLine("\nVous avez sélectionné le numéro 8 : Décrypter une image cachée");
                        Console.ReadKey();
                        image.AfficherImage(image.TrouveImageCachee(), fichier);
                        break;
                    case 9:
                        Console.WriteLine("Ouverture du rapport en .pdf");
                        Process.Start("Rapport Guillaume Turcas et Victor Lenoble.pdf");
                        break;
                }
                Console.WriteLine("\nTapez Escape pour sortir ou un numéro de commande à effectuer");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);
        }
        /// <summary>
        /// Pour l'affichage des matrices de filtre
        /// </summary>
        /// <param name="matrice"></param>
        static void AfficherMatrice(int[,] matrice)
        {
            if (matrice == null) Console.WriteLine("(matrice nulle)");
            else
            {
                int nbligne = matrice.GetLength(0);
                int nbcolonne = matrice.GetLength(1);
                if (matrice.Length == 0) Console.WriteLine("(matrice vide)");
                else
                {
                    for (int i = 0; i < nbligne; i++)
                    {
                        for (int j = 0; j < nbcolonne; j++)
                        {
                            if (matrice[i, j] > 0 && matrice[i, j] < 10) Console.Write(" ");
                            Console.Write(matrice[i, j] + " ");
                        }
                        Console.WriteLine();
                    }

                }
            }
        }

    }
}
