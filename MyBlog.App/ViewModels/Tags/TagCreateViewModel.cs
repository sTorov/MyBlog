﻿using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Tags
{
    public class TagCreateViewModel
    {
        [Display(Name = "PostId")]
        public int? PostId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя тега")]
        public string Name { get; set; }
    }
}
