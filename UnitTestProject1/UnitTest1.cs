using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BullsAndCows;

namespace UnitTestProject1
{
    [TestClass]
    public class TESTS_BAC
    {
        [TestMethod]
        public void NovoeChislo_GEN()
        {
            Form1 form = new Form1();

            // Act
            form.NovoeChislo();

            // Assert
            Assert.AreEqual(4, form.s.Length);
        }

        [TestMethod]
        public void SravnenieChisel_CMPR()
        {
            Form1 form = new Form1();
            form.s = "1234";  // Установка загаданного числа

            // Act
            form.textBox1.Text = "1243";  // Установка введенного числа
            form.SravenieChisel();

            // Assert
            Assert.AreEqual(2, form.polnoeSovpadenie);
            Assert.AreEqual(2, form.chastichnoeSovpadenie);
        }

        [TestMethod]

        public void RezultShow_DISP()
        {
            // Arrange
            Form1 form = new Form1();
            form.polnoeSovpadenie = 2;
            form.chastichnoeSovpadenie = 1;
            form.textBox1.Text = "1243";

            // Act
            form.RezultShow();

            // Assert
            Assert.AreEqual("Вы ввели: 1243\nБыков: 2\nКоров: 1\n", form.label2.Text);
        }
    }

}
