using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatHomePlugin
{
    public class StyleWechatModel
    {

        public string Title { get; set; }

        public List<StyleWechatChildModel> Child { get; set; } = new List<StyleWechatChildModel>();


        public class StyleWechatChildModel
        {
            public string Text { get; set; }

            public string Link { get; set; }
        }
    }
}
