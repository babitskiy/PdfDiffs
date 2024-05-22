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



/// <summary>
/// Наложение 2х изображений друг на друга.
/// </summary>
/// <param name="x">1е изображение.</param>
/// <param name="y">2е изображение.</param>
/// <param name="percent">Коэффициент прозрачности (от 0 до 1).</param>
/// <returns>Результат наложения двух изображений.</returns>
Bitmap AlphaBlending(Image x, Image y, float percent)
{
    if (percent < 0f || percent > 1f)
        throw new ArgumentOutOfRangeException();

    if (x == null || y == null)
        throw new NullReferenceException();

    Bitmap bmp = new Bitmap(
        Math.Max(x.Width, y.Width),
        Math.Max(x.Height, y.Height)
        );

    var cm = new ColorMatrix(
        new float[][] {
            new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
            new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
            new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
            new float[] { 0.0f, 0.0f, 0.0f, percent, 0.0f },
            new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
        }
    );

    using (var imgAttr = new ImageAttributes())
    {
        imgAttr.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        using (var g = Graphics.FromImage(bmp))
        {
            g.DrawImage(x, 0, 0, x.Width, x.Height);
            g.DrawImage(
                y,
                new Rectangle(0, 0, y.Width, y.Height),
                0, 0, y.Width, y.Height,
                GraphicsUnit.Pixel,
                imgAttr
                );
        }
    }

    return bmp;
}






//using GroupDocs.Comparison;

//using (Comparer comparer = new Comparer("t1.pdf"))
//{
//    comparer.Add("t2.pdf");
//    comparer.Compare("result.pdf");
//}
//Console.WriteLine("Done");








//using Aspose.Words;
//using Aspose.Words.Comparing;

//// Load PDF files
//Document PDF1 = new Document("t1.pdf");
//Document PDF2 = new Document("t2.pdf");

//// Convert PDF files to editable Word format
//PDF1.Save("t1.docx", SaveFormat.Docx);
//PDF2.Save("t2.docx", SaveFormat.Docx);

//// Load converted Word documents 
//Document DOC1 = new Document("t1.docx");
//Document DOC2 = new Document("t2.docx");

//// Set comparison options
//CompareOptions options = new CompareOptions();
//options.IgnoreFormatting = true;
//options.IgnoreHeadersAndFooters = true;
//options.IgnoreCaseChanges = true;
//options.IgnoreTables = true;
//options.IgnoreFields = true;
//options.IgnoreComments = true;
//options.IgnoreTextboxes = true;
//options.IgnoreFootnotes = true;

//// DOC1 will contain changes as revisions after comparison
//DOC1.Compare(DOC2, "user", DateTime.Today, options);

//if (DOC1.Revisions.Count > 0)
//    // Save resultant file as PDF
//    DOC1.Save("compared.pdf", SaveFormat.Pdf);
//else
//    Console.Write("Documents are equal");








//using iTextSharp.text.pdf;

//PdfReader reader1 = new PdfReader("t1.pdf");

//// Загружаем второй чертёж
//PdfReader reader2 = new PdfReader("t2.pdf");

//// Создаём список для хранения информации о первом чертеже
//List<object> objects1 = new();

//// Проходим по всем объектам первого чертежа и добавляем их в список
//for (int i = 1; i <= reader1.NumberOfPages; i++)
////{
//    PdfDictionary page = reader1.GetPageN(i);
//    PdfArray contents = page.GetAsArray(PdfName.CONTENTS);

//    for (int j = 0; j < contents.Count(); j++)
//    {
//        PdfObject obj = contents.GetPdfObject(j);
//        objects1.Add(obj);
//    }
//}

//// Аналогично создаём список для второго чертежа
//List<object> objects2 = new();
//// Проходим по всем объектам второго чертежа и добавляем их в список
//for (int i = 1; i <= reader2.NumberOfPages; i++)
//{
//    PdfDictionary page = reader2.GetPageN(i);
//    PdfArray contents = page.GetAsArray(PdfName.CONTENTS);

//    for (int j = 0; j < contents.Count(); j++)
//    {
//        PdfObject obj = contents.GetPdfObject(j);
//        objects2.Add(obj);
//    }
//}

//// Сравниваем списки объектов и выводим результаты
//Console.WriteLine("Различия между чертежами:");
//foreach (var obj in objects1)
//{
//    if (!objects2.Contains(obj))
//    {
//        Console.WriteLine($"Объект {obj} отсутствует во втором чертеже.");
//    }
//}
//foreach (var obj in objects2)
//{
//    if (!objects1.Contains(obj))
//    {
//        Console.WriteLine($"Объект {obj} присутствует только во втором чертеже.");
//    }
//}