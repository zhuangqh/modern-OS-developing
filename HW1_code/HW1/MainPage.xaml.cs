using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW1 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    private Cat cat;
    private Dog dog;
    private Pig pig;

    public MainPage() {
      this.InitializeComponent();
      cat = new Cat(board);
      dog = new Dog(board);
      pig = new Pig(board);
    }

    private delegate void AnimalSaying(object sender, EventArgs e);
    private event AnimalSaying Say;

    interface Animal {
      void saying(object sender, EventArgs e);
    }

    class Pig : Animal {
      TextBlock words;

      public Pig(TextBlock w) {
        this.words = w;
      }
      public void saying(object sender, EventArgs e) {
        this.words.Text += "pig: I am a pig\n"; 
      }
    }

    class Dog : Animal {
      TextBlock words;

      public Dog(TextBlock w) {
        this.words = w;
      }
      public void saying(object sender, EventArgs e) {
        this.words.Text += "dog: I am a dog\n";
      }
    }

    class Cat : Animal {
      TextBlock words;

      public Cat(TextBlock w) {
        this.words = w;
      }
      public void saying(object sender, EventArgs e) {
        this.words.Text += "cat: I am a cat\n";
      }
    }

    private void confirm_Click(object sender, RoutedEventArgs e) {
      string animalName = namePicker.Text.ToLower();
      namePicker.Text = string.Empty;
      if (animalName == "cat") {
        Say = new AnimalSaying(cat.saying);
      } else if (animalName == "dog") {
        Say = new AnimalSaying(dog.saying);
      } else if (animalName == "pig") {
        Say = new AnimalSaying(pig.saying);
      } else {
        return;
      }
      Say(this, new EventArgs());
    }

    private void saying_Click(object sender, RoutedEventArgs e) {
      Random rd = new Random();
      int character = rd.Next(3);
      if (character == 0) {
        Say = new AnimalSaying(cat.saying);
      } else if (character == 1) {
        Say = new AnimalSaying(dog.saying);
      } else if (character == 2) {
        Say = new AnimalSaying(pig.saying);
      }
      Say(this, new EventArgs());
    }

    private void namePicker_TextChanged(object sender, TextChangedEventArgs e) {

    }
  }

}
