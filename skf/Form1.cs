namespace skf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class Function
        {
            public delegate double FunctionDelegate(double x);

            public FunctionDelegate FunctionBody { get; set; }

            public Function(FunctionDelegate functionBody)
            {
                FunctionBody = functionBody;
            }

            public double Evaluate(double x)
            {
                return FunctionBody(x);
            }
        }

        public class TrapezoidalIntegration
        {
            public Function Function;
            public double LowerBound;
            public double UpperBound;
            public int NumberOfSteps;

            public TrapezoidalIntegration(Function function, double lowerBound, double upperBound, int numberOfSteps)
            {
                Function = function;
                LowerBound = lowerBound;
                UpperBound = upperBound;
                NumberOfSteps = numberOfSteps;
            }

            public double Integrate()
            {
                double stepSize = (UpperBound - LowerBound) / NumberOfSteps;

                double sum = 0;
                for (int i = 1; i < NumberOfSteps; i++)
                {
                    double x = LowerBound + i * stepSize;
                    sum += Function.Evaluate(x);
                }

                sum += (Function.Evaluate(LowerBound) + Function.Evaluate(UpperBound)) / 2;
                sum *= stepSize;

                return sum;
            }
        }

        private Function function;
        private TrapezoidalIntegration integration;


        private void CalculateIntegral()
        {
            string input = textBox1.Text;
            // Разбор входного текста и создание экземпляра Function, например:
            function = new Function(x => x*x*x);

            // Получение границ и количества шагов
            double lowerBound = Convert.ToDouble(textBox2.Text);
            double upperBound = Convert.ToDouble(textBox3.Text);
            int numberOfSteps = Convert.ToInt32(textBox4.Text);

            integration = new TrapezoidalIntegration(function, lowerBound, upperBound, numberOfSteps);
            double result = integration.Integrate();
            label1.Text = "Интеграл: " + result.ToString();
        }

        private void DrawAxes(Graphics g)
        {
            // Рисование осей
            int width = panel1.Width;
            int height = panel1.Height;
            Pen pen = new Pen(Color.Black, 1);

            g.DrawLine(pen, 0, height / 2, width, height / 2);
            g.DrawLine(pen, width / 2, 0, width / 2, height);
        }

        private void DrawFunction(Graphics g)
        {
            // Рисование графика функции
            if (function == null) return;

            int width = panel1.Width;
            int height = panel1.Height;
            Pen pen = new Pen(Color.Blue, 2);

            for (int i = 1; i < width; i++)
            {
                double x1 = (i - 1 - width / 2) / 50.0;
                double x2 = (i - width / 2) / 50.0;
                double y1 = function.Evaluate(x1) * 50;
                double y2 = function.Evaluate(x2) * 50;

                g.DrawLine(pen, i - 1, (float)(height / 2 - y1), i, (float)(height / 2 - y2));
            }
        }


        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawAxes(g);
            DrawFunction(g);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CalculateIntegral();
            panel1.Invalidate();
        }
    }
}