using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace kompasBadowski4c;

public partial class MainPage : ContentPage
{
    List<Stacje> stacje1 = new List<Stacje>();
    public class Stacje
    {
        public string id_stacji { get; set; }
        public string stacja { get; set; }
        public string data_pomiaru { get; set; }
        public string godzina_pomiaru { get; set; }
        public string temperatura { get; set; }
        public string predkosc_wiatru { get; set; }
        public string kierunek_wiatru { get; set; }
        public string wilgotnosc_wzgledna { get; set; }
        public string suma_opadu { get; set; }
        public string cisnienie { get; set; }
    }

    /*Location boston = new Location(42.358056, -71.063611);
    Location sanFrancisco = new Location(37.783333, -122.416667);

    double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);*/

    int kat = 0;
    double lat = 0;
    double lon = 0;
    float kw;
	public MainPage()
	{
		InitializeComponent();
        canvasView.Drawable = new GraphicsDrawable();
        ToggleCompass();
        nowy();
        GetCurrentLocation();
        getWiatr();
        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(3600000);
        timer.Tick += (s, e) => nowy();
        timer.Start();
    }

    private void ToggleCompass()
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                // Turn on compass
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
            }
            else
            {
                // Turn off compass
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
            }
        }
    }
    private async void getWiatr()
    {
        HttpClient client = new HttpClient();
        string text = await client.GetStringAsync("http://10.0.2.2:3000/latLon/" + lat + "/" + lon);
        for(int i = 0; i < stacje1.Count; i++)
        {
            if (stacje1[i].stacja == text)
            {
                kw = (float)Convert.ToDouble(stacje1[i].kierunek_wiatru);
            }
        }
    }
    public async Task GetCurrentLocation()
    {

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            Location location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        lat = location.Latitude;
        lon = location.Longitude;
    }
    public async Task<List<Stacje>> nowy()
    {
        HttpClient client = new HttpClient();
        string text = await client.GetStringAsync("http://10.0.2.2:3000/dane");
        stacje1 = JsonConvert.DeserializeObject<List<Stacje>>(text);
        getWiatr();
        return stacje1;
    }

    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        // Update UI Label with compass state
        label1.TextColor = Colors.Green;
        kat = Convert.ToInt32(e.Reading.HeadingMagneticNorth);
        label1.Text = $"{e.Reading}";
        canvasView.Drawable = new GraphicsDrawable
        {
            x = 200 + Convert.ToInt32(100 * Math.Cos(Math.PI * (0 - kat - 90) / 180)),
            y = 200 + Convert.ToInt32(100 * Math.Sin(Math.PI * (0 - kat - 90) / 180)),
            x2 = 200 + Convert.ToInt32(100 * Math.Cos(Math.PI * (kat - 90) / 180)),
            y2 = 200 + Convert.ToInt32(100 * Math.Sin(Math.PI * (kat - 90) / 180)),
            x3 = 200 + Convert.ToInt32(100 * Math.Cos(Math.PI * (kw - kat - 90) / 180)),
            y3 = 200 + Convert.ToInt32(100 * Math.Sin(Math.PI * (kw - kat - 90) / 180))
        };
        canvasView.Invalidate();
    }

    public class GraphicsDrawable : IDrawable
    {
        public float x { get; set; }
        public float y { get; set; }
        public float x2 { get; set; }
        public float y2 { get; set; }
        public float x3 { get; set; }
        public float y3 { get; set; }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 5;
            canvas.DrawLine(200, 200, x, y);
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine(200, 200, 200, 100);
            canvas.StrokeColor = Colors.LightBlue;
            canvas.DrawLine(200, 200, x3, y3);
        }
    }
}

