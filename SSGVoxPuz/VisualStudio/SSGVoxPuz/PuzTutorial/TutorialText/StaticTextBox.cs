using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    public class StaticTextBox : TextBox {
        
        
        protected override void DisplayExtended() {
            textField.text = GetFormattedText();    
        }

        protected override void CloseExtended() {
            
        }
    }
}
