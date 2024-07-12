namespace Shopping.lib.ViewModels
{
    public class SelectListViewModel
    {
        public SelectListViewModel()
        {
        }
        public SelectListViewModel(string text, string value)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}
