using KindFishman;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace kindFishman.Pages
{
    public class gradeModel : PageModel
    {
        [BindProperty]
        public float Length { get; set; }

        [BindProperty]
        public float Height { get; set; }
        
        [BindProperty]
        public float Width { get; set; }

        [BindProperty]
        public float weight { get; set; }
        public void OnGet()
        {
            //Load sample data
            var sampleData = new WeightModel.ModelInput()
            {
                Length = 31.2F,
                Height = 12.48F,
                Width = 4.3056F,
            };

            //Load model and predict output
            var result = WeightModel.Predict(sampleData);

            Length = result.Length;
            Height = result.Height;
            Width = result.Width;

            weight = result.Score;

        }

        public void OnPost()
        {
            //Load sample data
            var Userinputdata = new WeightModel.ModelInput()
            {
                Length = Length,
                Height = Height,
                Width = Width,
            };

            //Load model and predict output
            var result = WeightModel.Predict(Userinputdata);

            Length = result.Length;
            Height = result.Height;
            Width = result.Width;

            weight = result.Score;
        }
    }
}
