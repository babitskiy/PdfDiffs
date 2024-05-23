using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

// Создание экземпляров PdfDocument
PdfDocument pdf1 = new();
PdfDocument pdf2 = new();

// Загрузка образцов документов PDF
pdf1.LoadFromFile("t1.pdf");
pdf2.LoadFromFile("t2.pdf");

int minPagesCount = Math.Min(pdf1.Pages.Count, pdf2.Pages.Count);

// Просмотр каждой страницы PDF
for (int i = 0; i < minPagesCount; i++)
{
    // Преобразование всех страниц в изображения и установка разрешения на дюйм для изображений
    Image image1 = pdf1.SaveAsImage(i, PdfImageType.Bitmap, 500, 500);
    Image image2 = pdf2.SaveAsImage(i, PdfImageType.Bitmap, 500, 500);

    // Сохранить изображения в формате JPG в указанную папку 
    string fileJpg1 = @$"Image\\t1-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}-page{i + 1}.jpg";
    image1.Save(fileJpg1, ImageFormat.Jpeg);

    string fileJpg2 = @$"Image\\t2-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}-page{i + 1}.jpg";
    image2.Save(fileJpg2, ImageFormat.Jpeg);

    Bitmap source1 = new Bitmap(fileJpg1);
    Bitmap source2 = new Bitmap(fileJpg2);

    Console.WriteLine("Загрузка");

    //source1.MakeTransparent(); // Сделать фон прозрачным

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
            Console.WriteLine($"Загрузка страницы №{i + 1}. Прогресс: {x / 82} / {source1.Width / 82}");
    }
    source1.Save($"Image\\ComparedFile-{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}-page{i + 1}.jpg");
}