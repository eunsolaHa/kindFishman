using KindFishman;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net; // WebClient를 사용하기 위해 필요한 using 지시문 추가


namespace kindFishman.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public string imageUrl { get; set; }

        [BindProperty]
        public string species { get; set; }

        [BindProperty]
        public string ImgPath { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {

            string imgName = "fish.jpg";
            // 물리적 파일 경로를 구성합니다.
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imgName);

            // 샘플 데이터를 로드합니다.
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            ChoiceModel.ModelInput sampleData = new ChoiceModel.ModelInput()
            {
                ImageSource = imageBytes,
            };

            // 모델을 로드하고 예측을 수행합니다.
            var result = ChoiceModel.Predict(sampleData);

            species = result.PredictedLabel;
            ImgPath = imgName;

            // 예측 결과를 처리하거나 로그에 기록합니다.
            _logger.LogInformation("Prediction result: {0}, {1}", result.Score[0], result.Score[1]);
        }

        public void OnPost()
        {
            // imageUrl을 유효성 검사합니다. (더 많은 유효성 검사 로직을 추가할 수 있습니다)
            if (string.IsNullOrEmpty(imageUrl))
            {
                // imageUrl이 비어 있거나 유효하지 않은 경우 처리합니다.
                return;
            }

            // WebClient을 사용하여 주어진 URI에서 이미지를 다운로드합니다.
            using (var client = new WebClient())
            {
                try
                {
                    // 이미지 데이터를 저장하기 위해 MemoryStream을 사용합니다.
                    using (var imageStream = new MemoryStream(client.DownloadData(imageUrl)))
                    {
                        // MemoryStream에서 이미지 데이터를 읽어옵니다.
                        var imageBytes2 = imageStream.ToArray();

                        // 다운로드한 이미지를 사용하여 예측을 수행합니다.
                        ChoiceModel.ModelInput userData = new ChoiceModel.ModelInput()
                        {
                            ImageSource = imageBytes2,
                        };

                        // 모델을 로드하고 예측을 수행합니다.
                        var result = ChoiceModel.Predict(userData);

                        species = result.PredictedLabel;
                        ImgPath = imageUrl;

                        // 예측 결과를 로그로 남깁니다.
                        _logger.LogInformation("예측 결과: {0}, {1}", result.Score[0], result.Score[1]);
                    }
                }
                catch (Exception ex)
                {
                    // 이미지 다운로드 또는 예측 중에 발생할 수 있는 예외를 처리합니다.
                    _logger.LogError("이미지 처리 오류: " + ex.Message);
                }
            }
        }
    }
}
