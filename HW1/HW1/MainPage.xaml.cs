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
    public MainPage() {
      this.InitializeComponent();
    }

    interface Animal {
      string saying();
    }

    class Pig : Animal {
      public string saying() {
        return "I am a pig";
      }
    }

    class Dog : Animal {
      public string saying() {
        return "I am a dog";
      }
    }

    class Cat : Animal {
      public string saying() {
        return "I am a cat";
      }
    }

    private Cat cat;
    private Dog dog;
    private Pig pig;

    private void confirm_Click(object sender, RoutedEventArgs e) {
      string animalName = namePicker.Text;
      if (animalName == "cat") {
        cat = new Cat();
        board.Text += "cat: " + cat.saying() + "\n";
      } else if (animalName == "dog") {
        dog = new Dog();
        board.Text += "dog: " + dog.saying() + "\n";
      } else if (animalName == "pig") {
        pig = new Pig();
        board.Text += "pig: " + pig.saying() + "\n";
      }
      namePicker.Text = string.Empty;
    }

    private void namePicker_TextChanged(object sender, TextChangedEventArgs e) {

    }

    private void saying_Click(object sender, RoutedEventArgs e) {
      Random rd = new Random();
      int character = rd.Next(3);
      if (character == 0) {
        cat = new Cat();
        board.Text += "cat: " + cat.saying() + "\n";
      } else if (character == 1) {
        dog = new Dog();
        board.Text += "dog: " + dog.saying() + "\n";
      } else if (character == 2) {
        pig = new Pig();
        board.Text += "pig: " + pig.saying() + "\n";
      } else {
        board.Text += character + "\n";
      }
    }
  }

}
