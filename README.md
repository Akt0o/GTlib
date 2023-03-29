# Presentation
GTLib is a small library that allow you to create a pixelated glitch effect on your text on a canvas (in WPF).

To do so, go to the backend file (.cs) of your XAML window.
Create an instance of the class GlitchEffect (with no parameters in the constructor).
Next set your text like this :
```cs YourInstanceOfGlitchEffect.setText("your text","yourFont",30,"TheColorOfYourTextInHexa"); ```
(30 is the fontsize)

**The setText function MUST be used before the following functions.**

Next you will have to generate the frames, like this : 
```cs YourInstanceOfGlitchEffect.GenerateFrames(120,8,2); ```
(120 is the number of frames, 20 is the intensity of the effect, 2 is the offset of the pixels affect by the effect)

**The GenerateFrames function MUST be used before the following functions.**

Finally, we will use the function GetNextFrame to get the frames generated one by one.

If you only want one for a static text with no animation, do this : 
```cs YourCanvas.Background = YourInstanceOfGlitchEffect.GetNextFrame(); ```

If you want to animate your text with the effect fully, you can do something like this :

```cs
    public partial class MainWindow : Window
    {
      GlitchEffect testGlitch;
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += UpdateLoop;
            testGlitch = new GlitchEffect();
            testGlitch.setText("Hello World","Arial",30, "#1b3646");
            testGlitch.GenerateFrames(120,20,1);
        }
	
        public void UpdateLoop(object sender, EventArgs e)
        {
            testCanvas.Background = testGlitch.GetNextFrame();
        }
    }
```

gif example of the result :
![til](./Untilted.gif)
