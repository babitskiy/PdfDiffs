using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

//Создайте экземпляр PdfDocument
PdfDocument pdf1 = new();
PdfDocument pdf2 = new();

//Загрузка образца документа PDF
pdf1.LoadFromFile("t1.pdf");
pdf2.LoadFromFile("t2.pdf");

//Просматривайте каждую страницу в PDF
for (int i = 0; i < 1; i++)
{
    //Преобразуйте все страницы в изображения и установите разрешение на дюйм для изображений
    Image image1 = pdf1.SaveAsImage(i, PdfImageType.Bitmap, 500, 500);
    Image image2 = pdf2.SaveAsImage(i, PdfImageType.Bitmap, 500, 500);

    //Сохранить изображения в формате JPG в указанную папку 
    string fileJpg1 = @$"Image\\t1-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}.jpg";
    image1.Save(fileJpg1, ImageFormat.Jpeg);

    string fileJpg2 = @$"Image\\t2-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}.jpg";
    image2.Save(fileJpg2, ImageFormat.Jpeg);

    Bitmap source1 = new Bitmap(fileJpg1);
    Bitmap source2 = new Bitmap(fileJpg2);

    Console.WriteLine("Загрузка");

    //source1.MakeTransparent();
    for (int x = 0; x < source1.Width; x++)
    {
        for (int y = 0; y < source1.Height; y++)
        {
            Color currentColor1 = source1.GetPixel(x, y);
            Color currentColor2 = source2.GetPixel(x, y);

            if (currentColor1.R + currentColor1.G + currentColor1.B > 690
                && currentColor2.R + currentColor2.G + currentColor2.B < 300)
            {
                source1.SetPixel(x, y, Color.Red);
            }
        }

        if (x % 82 == 0)
            Console.WriteLine($"Загрузка {x / 82} / {source1.Width / 82}");
    }
    source1.Save($"Image\\ComparedFile-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}.jpg");
}