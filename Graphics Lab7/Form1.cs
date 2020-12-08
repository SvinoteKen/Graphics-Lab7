using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_Lab7
{
    public partial class Form1 : Form
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;
        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        // Графический контекст picturebox
        Graphics g_pictureBox;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты самолета
        int width = 355, height = 230; // Ширина и высота самолета
        Timer timer;
        public Form1() { InitializeComponent(); }
        // Функция рисования спрайта (автобуса)
        void DrawSprite()
        {
            //крыло 1
            g_sprite.RotateTransform(50);
            g_sprite.DrawEllipse(new Pen(Color.Black, 2), 140, -130, 120, 50);
            g_sprite.FillEllipse(new SolidBrush(Color.Red), 140, -130, 120, 50);
            g_sprite.ResetTransform();
            
            // хвост
            Point[] p1 = new Point[5] { new Point(30, 140), new Point(50, 70), new Point(80, 80), new Point(90, 150), new Point(30, 140) };
            g_sprite.RotateTransform(-10);
            g_sprite.FillPolygon(Brushes.Red, p1);
            g_sprite.DrawPolygon(new Pen(Color.Black, 2), p1);
            g_sprite.ResetTransform();
            //корпус
            g_sprite.DrawEllipse(new Pen(Color.Black,2), 50, 100, 300, 70);
            g_sprite.FillEllipse(new SolidBrush(Color.Red), 50, 100, 300, 70);
            //турбина
            g_sprite.DrawEllipse(new Pen(Color.Black,2), 50, 100, 30, 10);
            g_sprite.FillEllipse(new SolidBrush(Color.Red), 50, 100, 30, 10);
            //Иллюминаты
            for (int x = 120; x <= 240; x += 40)
            {
                g_sprite.DrawEllipse(new Pen(Color.Black, 2), x, 120, 15, 15);
                g_sprite.FillEllipse(new SolidBrush(Color.White), x, 120, 15, 15);
            }
            //Окно
            g_sprite.RotateTransform(5);
            g_sprite.DrawEllipse(new Pen(Color.Black, 2), 300, 85, 50, 15);
            g_sprite.FillEllipse(new SolidBrush(Color.White), 300, 85, 50, 15);
            g_sprite.ResetTransform();
            //крыло 2
            g_sprite.RotateTransform(-40);
            g_sprite.DrawEllipse(new Pen(Color.Black, 2), -30, 240, 120, 50);
            g_sprite.FillEllipse(new SolidBrush(Color.Red), -30, 240, 120, 50);
            g_sprite.ResetTransform();
        }
        // Функция сохранения части изображения шириной
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =pictureBoxBitMap.PixelFormat;
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём Bitmap для pictureBox1 и графический контекст
            pictureBox1.Image = Image.FromFile(@"C:\Users\MSI\Desktop\fon.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            // Рисуем линию движения автобуса
            g_pictureBox.FillRectangle(new SolidBrush(Color.Black), 0, 430,
            pictureBox1.Width-1, pictureBox1.Height-1);
            // Рисуем автобус на графическом контексте g_sprite
            DrawSprite();
            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            // Задаем начальные координаты вывода движущегося объекта
            x = 1; y = 200;
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            // Выводим автобус на графический контекст g_pictureBox
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        // Обрабатываем событие от таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x, y);
            // Изменяем координаты для следующего вывода автобуса
            x += 10;
            if (x > 200) { y -= 2; }
            // Проверяем на выход изображения автобуса за правую границу
            if (x > pictureBox1.Width - 1) { x = pictureBox1.Location.X; y=200;}
            // Сохраняем область экрана перед первым выводом автобуса
            SavePart(x, y);
            // Выводим самолета
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
        }
    }

}


