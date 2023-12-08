using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BullsAndCows
{
    public partial class Form1 : Form
    {
        public Random rand = new Random();
        public int[] x = new int[4];
        // в строковой переменной впоследствии будем хранить строковое представление загаданного числа
        public string s;
        public int Attempts;
        public string userName;
        public string itemText2;
        public string helpFilePath = "help.txt";
        // счетчики полного и частичного совпадения цифр в загаданном и введенном числах
        public int polnoeSovpadenie;
        public int chastichnoeSovpadenie;



        public Form1()
        {
            InitializeComponent();
            userName = Interaction.InputBox("Введите текст:");
            
            

            LoadDataFromFile();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            NewGame();
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Имя пользователя не может быть пустым. Закрываю приложение.");
                Application.Exit();
            }
        }



        public void NewGame()
        {
            // вызов метода для генерации нового числа
            NovoeChislo();
            // обнуление метки вывода результатов сравнения загаданного и введенного чисел
            label2.Text = "";
            // обнуление метки вывода загаданного числа
            label4.Text = "";
            // открываем textbox для ввода значений
            textBox1.ReadOnly = false;
            Attempts = 0;

        }

        // метод генерации нового числа
        public void NovoeChislo()
        {

            // флаг сравнения с предыдущими цифрами. совпадает - true 
            bool contains;
            // цикл заполнения массива нового числа новыми цифрами
            for (int i = 0; i < 4; i++)
            {
                do
                {
                    contains = false;
                    // генерация новой цифры
                    x[i] = rand.Next(10);
                    // цикл сравнения сгенерированной цифры с предыдущими
                    for (int k = 0; k < i; k++)
                        if (x[k] == x[i])
                            //если сгенериррованная цифра совпала с одной из предыдущих
                            // флаг сравнения делаем true для продолжения генерации
                            //несовпадающей цифры в элемент массива
                            contains = true;
                } while (contains);
            }
            s = x[0].ToString() + x[1] + x[2] + x[3];// из элементов массива формируем строку
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back)
                // если были нажаты цифра или бекспейс, то событие обработать в обычном режиме
                e.Handled = false;
            else
                //иначе, поставить метку что событие обработанно, но не пускать сигнал в текстбокс
                e.Handled = true;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 4)
            {
                // вывести сообщение об ошибке
                MessageBox.Show("введенное число должно быть четырехзначным");
            }
            else
            {
                // иначе вызвать метод сравнения чисел
                SravenieChisel();
                // вызвать метод вывода результатов сравнения на экран
                RezultShow();
            }
            Attempts++;
            label4.Text = ("Попытки: " + Attempts);
            // очистка текстбокса
            textBox1.Text = "";

            if (polnoeSovpadenie == 4)
            {
                MessageBox.Show("Поздравляем! Вы выиграли за " + Attempts + " шагов!");

                AddItemAndSubitem(userName);
            }
        }

        public void RezultShow()
        {
            label2.Text = "Вы ввели: " + textBox1.Text + "\n" + "Быков: " + polnoeSovpadenie + "\n" + "Коров: " + chastichnoeSovpadenie + "\n";
        }


        // метод сравнения загаданного и введенного чисел
        public void SravenieChisel()
        {
            // обнуление счетчиков
            polnoeSovpadenie = 0;
            chastichnoeSovpadenie = 0;
            // перевод содержимого текстбокса в символьный массив
            char[] ch = textBox1.Text.ToCharArray();
            // цикл проверки символов в массиве
            for (int i = 0; i < 4; i++)
            {
                // если строка s содержит в себе элемент массива
                if (s.Contains(ch[i]))
                {
                    // если номер символа в массиве совпадает с номером символа в строке
                    if (s[i] == ch[i])
                        // увеличиваем счетчик полного совпадения
                        polnoeSovpadenie++;
                    else
                        // если номер символа в массиве не совпадает с номером символа в строке
                        // увеличиваем счетчик неполного совпадения
                        chastichnoeSovpadenie++;
                }
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            label2.Text = ("Ответ: " + s);
            // обнуляем метку вывода результатов сравнения введенного и загаданного чисел

            // запрещаем ввод символов в текстбокс
            textBox1.ReadOnly = true;

        }

        public void button3_Click(object sender, EventArgs e)
        {
            NewGame();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void SaveDataToFile()
        {
            using (StreamWriter writer = new StreamWriter("saveData.txt"))
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    // Записываем значения в файл, разделяя их табуляцией
                    writer.WriteLine($"{item.Text}\t{item.SubItems[1].Text}");
                }
            }
        }


        public void LoadDataFromFile()
        {
            if (File.Exists("readData.txt"))
            {
                List<string[]> lines = new List<string[]>();

                using (StreamReader reader = new StreamReader("readData.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Разделяем значения по табуляции и добавляем их в список
                        string[] values = line.Split('\t');
                        lines.Add(values);
                    }
                }

                // Добавляем значения из списка в ListView
                foreach (var values in lines)
                {
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
            }
        }
        private void AddItemAndSubitem(string itemText)
        {
            // Создаем новый элемент ListView
            ListViewItem newItem = new ListViewItem(itemText);
            
            // Добавляем подэлемент к элементу
            itemText2 = Attempts.ToString();
            newItem.SubItems.Add(itemText2);
            listView1.Items.Add(newItem);
        }
        public void ShowHelp()
        {
            try
            {
                string helpText = File.ReadAllText(helpFilePath);
                MessageBox.Show(helpText, "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла справки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }




        }

        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Сохранение данных при закрытии формы
            
                // Запрашиваем у пользователя сохранение игры
                DialogResult result = MessageBox.Show("Хотите сохранить игру перед выходом?", "Сохранение игры", MessageBoxButtons.YesNoCancel);

                // Обрабатываем результат
                switch (result)
                {
                    case DialogResult.Yes:
                        // Код для сохранения игры
                        SaveDataToFile();
                    File.Copy("saveData.txt", "readData.txt", true);
                    break;

                    case DialogResult.No:
                        // Ничего не делаем, просто закрываем форму
                        break;

                    case DialogResult.Cancel:
                        // Отменяем закрытие формы
                        e.Cancel = true;
                        break;
                }
            
            
        }

        public void button4_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Хотите удалить файл сохранения?", "УДАЛЕНИЕ!", MessageBoxButtons.YesNoCancel);

            // Обрабатываем результат
            switch (result)
            {
                case DialogResult.Yes:
                    File.Delete("readData.txt");
                    if (File.Exists("saveData.txt"))
                    {
                        try
                        {
                            // Удаляем файл
                            File.Delete("saveData.txt");
                            
                            MessageBox.Show("Файл успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Файл не существует.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case DialogResult.No:
                    // Ничего не делаем, просто закрываем форму
                    break;

                case DialogResult.Cancel:
                    // Отменяем закрытие формы
                    
                    break;
            }
        }
    }
}
