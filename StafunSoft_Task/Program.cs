using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShtafunSoft_Task.Structs;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int rowCounter = 0; // подсчет строк для создания уникальных классов

// переменные для создания html документа. head для <head>, body - <body>
string head = "";
string body = "<div class=\"main\">";

app.MapGet("/", async (context) =>
{
    // некоторые данные
    var r1 = new Row();
    r1.Add(Img("1")).Add(Img("2")).Add(Img("3"));
    var col1 = new Column();
    col1.Add(Img("4")).Add(Img("5")).Add(Img("6"));

    var r2 = new Row();
    r2.Add(col1).Add(r1);

    // функция отрисовки
    drawStoryBoard(r2, new Paddings(_width: 1600, _top: 10, _bot: 12, _left: 13));

    // закрытие тегов
    head += "</style></head>";
    body += "</div>";

    // соединение head и body
    string draw = head + body;

    // отправка
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(draw);
});

app.UseStaticFiles();
app.Run();

// возвращает src для тега img
string Img(string name)
{
    return $"\"images/{name}.jpg\"";
}

void drawStoryBoard(object obj, Paddings? paddings = null) {
    try {

        if (head == "")
        {
            // объявление head, если значение еще не установлено
            head = "<head>" +
                        "<style> " +
                        "body {background-color: gray;}" +
                        "div.col {display: flex; flex-direction: column; justify-content: space-between;}" +
                        $"div.col img {{min-width: calc(100% - {paddings?.paddingLeft + paddings?.paddingRight}px);}}" +
                        $"div.main {{max-width: {paddings?.width}px; }}div.col img{{ max-width: 100%; }}" +
                        $"img {{ max-width: 100%; object-fit: cover; padding: {paddings?.paddingTop}px {paddings?.paddingRight}px {paddings?.paddingBot}px {paddings?.paddingLeft}px; }}";
        }

        // если нужно отрисовать строку
        if (obj.GetType() == typeof(Row))
        {
            Row _row = (Row)obj;
            int currentCount = _row.objects.Count > 0 ? _row.objects.Count : 1; // получаем кол-во элементов в строке

            // производим вставку стилей в head для новой строки
            head += $"div.row{rowCounter} {{display: flex; flex-direction: row; }}" +
                $"div.row{rowCounter} div {{ max-width: calc(100% / {currentCount}); }}" +
                $"div.row0 img {{ max-width: calc(100% / {currentCount} - {paddings?.paddingLeft + paddings?.paddingRight}px);}}";
            
            // добавляем строку в body
            body += $"<div class=\"row{rowCounter}\">";
            rowCounter++;
            foreach (var el in _row.objects)
            {
                // рекурсия на элементы строки
                drawStoryBoard(el, paddings);
            }
            // закрытие тега
            body += "</div>";
            return; // возврат
        }

        // если пришла на вход столбец
        if (obj.GetType() == typeof(Column))
        {
            Column _col = (Column)obj;
            // добавляем его в body
            body += "<div class=\"col\">";
            foreach (var el in _col.objects)
            {
                // рекурсивная обработка элементов столбца
                drawStoryBoard(el, paddings);
            }
            body += "</div>";
            return;
        }

        // случай, если пришла картинка (строка с названием)
        string _pic = (string)obj;
        body += $"<img src={_pic}>"; // добавлением изображения с его src
    } catch(Exception e)
    {
        // на всякий случай
        body += $"<div>{e.Message}</div>";
    }
}

// вспомогательный класс для установки отступов
class Paddings
{
    public int paddingTop { get; set; }
    public int paddingBot { get; set; }
    public int paddingLeft { get; set; }
    public int paddingRight { get; set; }
    public int width { get; set; }
    public Paddings(int _top = 0, int _bot = 0, int _left = 0, int _right = 0, int _width = 0)
    {
        paddingTop = _top;
        paddingBot = _bot;
        paddingLeft = _left;
        paddingRight = _right;
        width = _width;
    }
}


