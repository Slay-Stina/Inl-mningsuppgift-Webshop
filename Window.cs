using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

public class Window
{
    public string Header { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public List<string> TextRows { get; set; } = new List<string> { "" };
    public int? SelectedIndex { get; set; }
    public Window(int left, int top)
    {
        Left = left;
        Top = top;
    }
    public Window(string header, int left, int top)
    {
        Header = header;
        Left = left;
        Top = top;
    }
    public Window(string header, string errorMsg)
    {
        Header = header;
        Left = 50;
        Top = 20;
        TextRows = new List<string> { errorMsg };
    }
    public Window(string header, List<string> list)
    {
        Header = header;
        Left = 50;
        Top = 20;
        TextRows = list;
    }
    public Window(string header, int left, int top, List<string> textRows)
    {
        Header = header;
        Left = left;
        Top = top;
        TextRows = textRows;
    }

    public void Draw()
    {
        var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;

        // Kolla om Header är längre än det längsta ordet i listan
        if (width < Header.Length + 4)
        {
            width = Header.Length + 4;
        };

        // Rita Header
        Console.SetCursorPosition(Left, Top);
        if (Header != "")
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new String('─', width - Header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new String('─', width + 2) + '┐');
        }

        // Rita raderna i sträng-Listan
        for (int j = 0; j < TextRows.Count; j++)
        {
            Console.SetCursorPosition(Left, Top + j + 1);
            Console.Write('│' + " ");
            if (j == SelectedIndex)
            {
                SelectedRow();
            }
            else
            {
                Console.ResetColor();
            }
            Console.Write(TextRows[j] + new String(' ', width - TextRows[j].Length));
            Console.ResetColor();
            Console.WriteLine(" " + '│');
            //Console.WriteLine('│' + " " + TextRows[j] + new String(' ', width - TextRows[j].Length + 1) + '│');
        }

        // Rita undre delen av fönstret
        Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
        Console.Write('└' + new String('─', width + 2) + '┘');


        // Kolla vilket som är den nedersta posotion, i alla fönster, som ritats ut
        if (Lowest.LowestPosition < Top + TextRows.Count + 2)
        {
            Lowest.LowestPosition = Top + TextRows.Count + 2;
        }

        Console.SetCursorPosition(0, Lowest.LowestPosition);
    }
    public static void SelectedRow()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }
    public void Navigate()
    {
        if (TextRows == null || TextRows.Count == 0)
            return;
        if (SelectedIndex is null)
        {
            SelectedIndex = 0;
        }

        if (Program.KeyInfo.Key == ConsoleKey.UpArrow)
        {
            SelectedIndex = (SelectedIndex == 0)
                ? TextRows.Count - 1
                : SelectedIndex - 1;
        }
        else if (Program.KeyInfo.Key == ConsoleKey.DownArrow)
        {
            SelectedIndex = (SelectedIndex == TextRows.Count - 1)
                ? 0
                : SelectedIndex + 1;
        }
        
        Draw();
    }
}

public static class Lowest
{
    public static int LowestPosition { get; set; }
}

