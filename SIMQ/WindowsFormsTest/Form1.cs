using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIMQ;
using SIMQ.Distributions;
using SIMQ.testsGenerators;

namespace WindowsFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void selectDistibution_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            switch (selectDistibution.SelectedIndex)
            {
                case 0:
                    label3.Text = "Вероятность «успеха»";
                    label4.Text = "Число «испытаний»";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
                case 1:
                    label3.Text = "Среднее количество успешных испытаний";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 2:
                    label3.Text = "Вероятность «успеха»";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 3:
                    label3.Text = "Вероятность «успеха»";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 4:
                    label3.Text = "Кол-во всех элементов";
                    label4.Text = "Число «испытаний»";
                    label5.Text = "Кол-во интересующих элементов";
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    numericUpDown3.Visible = true;
                    break;
                case 5:
                    label3.Text = "Среднее число наступлений события в единицу времени";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 6:
                    label3.Text = "Коэффициент сдвига";
                    label4.Text = "Коэффициент масштаба";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
                case 7:
                    label3.Text = "k";
                    label4.Text = "theta";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
                case 8:
                    label3.Text = "alpha";
                    label4.Text = "beta";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
                case 9:
                    label3.Text = "p";
                    label4.Text = "r";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
                case 10:
                    label3.Text = "sigma";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 11:
                    label3.Text = "a";
                    label3.Visible = true;
                    numericUpDown1.Visible = true;
                    break;
                case 12:
                    label3.Text = "a";
                    label4.Text = "b";
                    label3.Visible = true;
                    label4.Visible = true;
                    numericUpDown1.Visible = true;
                    numericUpDown2.Visible = true;
                    break;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();

            //var rndClassic = new BaseSensor(0);
            //for (int i = 0; i < 100; i++)
            //{
            //    var rnd = new BaseSensor(i);
            //    int value = 0;
            //    value = (int)rnd.Next(0, 100);
            //    chart2.Series[0].Points.AddXY(i, value);
            //    chart2.Series[1].Points.AddXY(i, value);
            //    var valueClassic = rndClassic.Next(0, 100);
            //    chart1.Series[0].Points.AddXY(i, valueClassic);
            //    chart1.Series[1].Points.AddXY(i, valueClassic);
            //}

            IDistribution distrib = null;
            switch (selectDistibution.SelectedIndex)
            {
                case 0:
                    var p = (double)numericUpDown1.Value;
                    var n = (int)numericUpDown2.Value;
                    distrib = new BinomialDistribution(p, n);
                    break;
                case 1:
                    var rate = (double)numericUpDown1.Value;
                    distrib = new PoissonDistribution(rate);
                    break;
                case 2:
                    p = (double)numericUpDown1.Value;
                    distrib = new GeometricDistibution(p);
                    break;
                case 3:
                    p = (double)numericUpDown1.Value;
                    distrib = new BernoulliDistribution(p);
                    break;
                case 4:
                    var N = (int)numericUpDown1.Value;
                    n = (int)numericUpDown2.Value;
                    var K = (int)numericUpDown3.Value;
                    distrib = new HypergeometricDistribution(N, n, K);
                    break;
                case 5:
                    rate = (double)numericUpDown1.Value;
                    distrib = new ExponentialDistribution(rate);
                    break;
                case 6:
                    var mu = (double)numericUpDown1.Value;
                    var sigma = (double)numericUpDown2.Value;
                    distrib = new NormalDistribution(mu, sigma);
                    break;
                case 7:
                    var k = (double) numericUpDown1.Value;
                    var theta = (double)numericUpDown2.Value;
                    distrib = new GammaDistribution(k, theta);
                    break;
                case 8:
                    var alpha = (double)numericUpDown1.Value;
                    var beta = (double)numericUpDown2.Value;
                    distrib = new BetaDistribution(alpha, beta);
                    break;
                case 9:
                    p = (double)numericUpDown1.Value;
                    var r = (int)numericUpDown2.Value;
                    distrib = new PascalDistribution(p, r);
                    break;
                case 10:
                    sigma = (double)numericUpDown1.Value;
                    distrib = new RayleighDistribution(sigma);
                    break;
                case 11:
                    var a = (double)numericUpDown1.Value;
                    distrib = new TDistribution(a);
                    break;
                case 12:
                    a = (double)numericUpDown1.Value;
                    var b = (double)numericUpDown2.Value;
                    distrib = new FDistribution(a, b);
                    break;

            }

            if (distrib is null) return;

            var countValue = (int)this.count.Value;
            var arr = new int[1000];
            for (int i = 0; i < countValue; i++)
            {
                var value = distrib.Generate();
                if (value >= 100)
                    continue;
                var index = Math.Round(value, 0, MidpointRounding.AwayFromZero);
                arr[(int)(index)]++;
            }

            double countDistrib = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                {
                    var yValue = (double)(arr[i]) / countValue;
                    chart1.Series[0].Points.AddXY(i, yValue);
                    chart1.Series[1].Points.AddXY(i, yValue);
                    countDistrib += yValue;
                    chart2.Series[0].Points.AddXY(i, countDistrib);
                    chart2.Series[1].Points.AddXY(i, countDistrib);
                }
            }
        }
    }
}
