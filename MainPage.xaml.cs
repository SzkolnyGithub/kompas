namespace zadanie22._02._24Badowski4c;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
	}

    public class GraphicsDrawable : IDrawable 
	{
        public float x2 { get; set; }
        public float y2 { get; set; }
        public float r { get; set; }
        public void Draw(ICanvas canvas, RectF dirtyRect)
		{
			//Random rand = new Random();
			canvas.StrokeColor = Colors.Black;
			canvas.StrokeSize = 5;
			canvas.DrawLine(200, 200, 200, 10); // punkt 1 : (200, 200) punkt 2 : (200, 10) - dluga kreska
            canvas.DrawLine(200, 200, x2, y2);
            canvas.Rotate(r);
            //canvas.StrokeSize = 8;
            //canvas.DrawLine(rand.Next(10, 300), rand.Next(10, 300), rand.Next(10, 200), rand.Next(10, 200)); // punkt 1 : (200, 200) punkt 2 : (250, 130) - krotka kreska
        }
	}

	private void klik(object sender, EventArgs e)
	{
        string[] coords = new string[4];
        if (coordinates.Text == "" || coordinates.Text == null)
		{
            canvasView.Drawable = new GraphicsDrawable
            {
                x2 = 200f,
                y2 = 150f
            };
            return;
        } 
        else
        {
            coords = coordinates.Text.Split(";");
            canvasView.Drawable = new GraphicsDrawable
            {
                x2 = (float)Convert.ToDouble(coords[0]),
                y2 = (float)Convert.ToDouble(coords[1])  // ma sie krecic w zaleznosci od podanego kata
            };
        }
		
		
		canvasView.Invalidate();
    }
}

