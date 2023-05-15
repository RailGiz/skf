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
            function = new Function(x => 1/x);

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

        private const double MAX_DRAW_VALUE = 1E5; // Максимальное значение для рисования графика

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

                if (double.IsInfinity(y1))
                {
                    y1 = Math.Sign(y1) * MAX_DRAW_VALUE;
                }
                if (double.IsInfinity(y2))
                {
                    y2 = Math.Sign(y2) * MAX_DRAW_VALUE;
                }

                g.DrawLine(pen, i - 1, (float)(height / 2 - y1), i, (float)(height / 2 - y2));
            }
        }

        private void DrawTrapezoids(Graphics g)
        {
            if (integration == null || function == null) return;

            int width = panel1.Width;
            int height = panel1.Height;
            Pen pen = new Pen(Color.Red, 1);

            double stepSize = (integration.UpperBound - integration.LowerBound) / integration.NumberOfSteps;
            for (int i = 0; i < integration.NumberOfSteps; i++)
            {
                double x1 = integration.LowerBound + i * stepSize;
                double x2 = integration.LowerBound + (i + 1) * stepSize;
                double y1 = function.Evaluate(x1);
                double y2 = function.Evaluate(x2);

                int screenX1 = (int)((x1 * 50) + width / 2);
                int screenX2 = (int)((x2 * 50) + width / 2);
                int screenY1 = height / 2 - (int)(y1 * 50);
                int screenY2 = height / 2 - (int)(y2 * 50);

                if (double.IsInfinity(y1))
                {
                    y1 = Math.Sign(y1) * MAX_DRAW_VALUE;
                }
                if (double.IsInfinity(y2))
                {
                    y2 = Math.Sign(y2) * MAX_DRAW_VALUE;
                }

                screenY1 = Math.Max(screenY1, height / 2 - (int)(MAX_DRAW_VALUE * 50));
                screenY2 = Math.Max(screenY2, height / 2 - (int)(MAX_DRAW_VALUE * 50));

                g.DrawLine(pen, screenX1, screenY1, screenX2, screenY2); // Верхняя сторона трапеции
                g.DrawLine(pen, screenX1, screenY1, screenX1, height / 2); // Левая сторона трапеции
                g.DrawLine(pen, screenX2, screenY2, screenX2, height / 2); // Правая сторона трапеции
            }
        }
        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawAxes(g);
            DrawFunction(g);
            DrawTrapezoids(g);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CalculateIntegral();
            panel1.Invalidate();
        }
    }
}