using MaterialSkin;

namespace Megafon.Contracts.Models;

public class ThemeBundle
{
        public MaterialSkinManager.Themes Theme { get; set; }
        public Primary Main { get; set; }
        public Primary LightMain { get; set; }
        public Primary DarkMain { get; set; }
        public Accent Accent { get; set; }
}
