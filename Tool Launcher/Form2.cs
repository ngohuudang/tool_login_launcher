using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FoodAndPetFarms
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        public class Food
        {
            public string Name { get; set; }
            public int Price { get; set; }
        }

        //public void AddBinding()
        //{
        //    textBox1.DataBindings.Add(new Binding("Text", comboBox1.DataSource, "Price"));
        //}
        List<Food> listItem;
        Food food;


        public Form2()
        {
            InitializeComponent();
            listItem = new List<Food>()
            {
                new Food(){Name = "Táo", Price = 2},
                new Food(){Name = "Dưa", Price = 3},
                new Food(){Name = "Đào", Price = 4},
                new Food(){Name = "Sâm", Price = 5}
            };
            comboBox1.DataSource = listItem;
            comboBox1.DisplayMember = "Name";


            //AddBinding();
        }

        private int tinhThucAn(int n_thu,int n_thucAn, Food food)
        {
            int sum = 0;
            int thucAnCan = n_thucAn;
            switch (food.Price)
            {
                case 2:
                    while (thucAnCan > 5)
                    {
                        sum += thucAnCan / 2;
                        thucAnCan /= 2;
                    }
                    break;
                case 3:
                    while (thucAnCan > 8)
                    {
                        sum += thucAnCan / 3;
                        thucAnCan /= 3;
                    }
                    break;
                case 4:
                    while (thucAnCan > 10)
                    {
                        sum += thucAnCan / 4;
                        thucAnCan /= 4;
                    }
                    break;
                case 5:
                    while (thucAnCan > 13)
                    {
                        sum += thucAnCan / 5;
                        thucAnCan /= 5;
                    }
                    break;
            }
            return n_thu * 5 - sum < 1 ? n_thucAn / food.Price : tinhThucAn(n_thu, n_thucAn + ((n_thu * 5 - sum) <= 1 ? 1 : (n_thu * 5 - sum) - 1), food);
        }

        private int tinhThu(int n_thucAn,Food food)
        {
            int thucAnTaoRa = n_thucAn;
            switch(food.Price)
            {
                case 2:
                    while (n_thucAn >= 10)
                    {
                        thucAnTaoRa += n_thucAn / 2;
                        n_thucAn /= 2;
                    }
                    break;
                case 3:
                    while (n_thucAn >= 15)
                    {
                        thucAnTaoRa += n_thucAn / 3;
                        n_thucAn /= 3;
                    }
                    break;
                case 4:
                    while (n_thucAn >= 20)
                    {
                        thucAnTaoRa += n_thucAn / 4;
                        n_thucAn /= 4;
                    }
                    break;
                case 5:
                    while (n_thucAn >= 25)
                    {
                        thucAnTaoRa += n_thucAn / 5;
                        n_thucAn /= 5;
                    }
                    break;
            }
            return thucAnTaoRa/5;
        }

        private void run(Food food)
        {
            int n;
            if (textBox3.TextLength != 0)
            {
                n = int.Parse(textBox3.Text);
                MessageBox.Show(tinhThu(n, food).ToString(), "Số thú nuôi");
            }
            if (textBox2.TextLength != 0)
            {
                n = int.Parse(textBox2.Text);
                MessageBox.Show(tinhThucAn(n, n * 5, food).ToString(), "Số thức ăn cần");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            run(food);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 settings = new Form2();
            this.Close();
            settings.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedValue != null)
            {
                food = cb.SelectedValue as Food;
            }
        }
    }
}
