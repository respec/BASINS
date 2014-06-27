/**
 * ExcelAdapter enables Copy-Paste Clipboard functionality on JTables.
 * The clipboard data format used by the adapter is compatible with
 * the clipboard format used by Excel. This provides for clipboard
 * interoperability between enabled JTables and Excel.
 * 
 * Note:  this code based off of the ExcelAdapter example from
 * http://www.javaworld.com/javaworld/javatips/jw-javatip77.html
 */
public class ExcelAdapter //implements ActionListener
   {
   protected string rowstring,value;
   protected System.Windows.Forms.Clipboard system;
   protected StringSelection stsel;
   protected atcControls.atcGridSource jTable1 ;
   protected string calculatorType;
   //protected JTextArea jTArea;
   //added string to enable others to paste into 
   private string importedContent;
   /**
    * The Excel Adapter is constructed with a
    * JTable on which it enables Copy-Paste and acts
    * as a Clipboard listener.
    */
public ExcelAdapter(atcControls.atcGridSource myJTable, string calculatorType)
   {
      jTable1 = myJTable;
      KeyStroke copy = KeyStroke.getKeyStroke(KeyEvent.VK_C,ActionEvent.CTRL_MASK,false);
      // Identifying the copy KeyStroke user can modify this
      // to copy on some other Key combination.
      KeyStroke paste = KeyStroke.getKeyStroke(KeyEvent.VK_V,ActionEvent.CTRL_MASK,false);
      // Identifying the Paste KeyStroke user can modify this
      //to copy on some other Key combination.
jTable1.registerKeyboardAction(this,"Copy",copy,JComponent.WHEN_FOCUSED);
jTable1.registerKeyboardAction(this,"Paste",paste,JComponent.WHEN_FOCUSED);
     // system = Toolkit.getDefaultToolkit().getSystemClipboard();
      this.calculatorType = calculatorType;
   }
   /**
    * Public Accessor methods for the Table on which this adapter acts.
    */
public atcControls.atcGridSource getJTable() {return jTable1;}
public void setJTable(atcControls.atcGridSource jTable1) {this.jTable1=jTable1;}
//public void setResultsTextArea(JTextArea jta){this.jTArea = jta;}

    /**
    * When this method is called, it either copies or pastes information into intermediate locations.
    * 
    * Originally, this method is activated on the Keystrokes we are listening to
    * in this implementation. Here it listens for Copy and Paste ActionCommands.
    * Selections comprising non-adjacent cells result in invalid selection and
    * then copy action cannot be performed.
    * Paste is done by aligning the upper left corner of the selection with the
    * 1st element in the current selection of the JTable.
    */
public void actionPerformed()//ActionEvent e)
   {
      if (e.getActionCommand().compareTo("Copy")==0)
      {
         StringBuffer sbf=new StringBuffer();
         // Check to ensure we have selected only a contiguous block of
         // cells
         //int numcols=jTable1.getSelectedColumnCount();
         int numcols=jTable1.getColumnCount(); // sri- jul 31 2012
        // int numrows=jTable1.getSelectedRowCount();
         int numrows=jTable1.getRowCount(); // sri- jul 31 2012

         jTable1.selectAll();  // sri- jul 31 2012

         int[] rowsselected=jTable1.getSelectedRows();
         int[] colsselected=jTable1.getSelectedColumns();
         if (!((numrows-1==rowsselected[rowsselected.length-1]-rowsselected[0] &&
                numrows==rowsselected.length) &&
(numcols-1==colsselected[colsselected.length-1]-colsselected[0] &&
                numcols==colsselected.length)))
         {
            JOptionPane.showMessageDialog(null, "Invalid Copy Selection",
                                          "Invalid Copy Selection",
                                          JOptionPane.ERROR_MESSAGE);
            return;
         }
         for (int i=0;i<numrows;i++)
         {
            for (int j=0;j<numcols;j++)
            {
            	double newValue = 0d;
            	try{
            	newValue = double.parseDouble((jTable1.getValueAt(rowsselected[i],colsselected[j]).toString()));
            	if (newValue.equals(double.NaN)){
            		newValue = 0d;
            	}
            	//sbf.append(jTable1.getValueAt(rowsselected[i],colsselected[j]));
            	sbf.append(newValue);
               if (j<numcols-1) sbf.append("\t");
            	}
            	
            	catch (Exception exc){
            		
            	}
            }
            sbf.append("\n");
         }
         //stsel  = new StringSelection(sbf.toString());
        // system = Toolkit.getDefaultToolkit().getSystemClipboard();
         //system.setContents(stsel,stsel);
         this.jTArea.setText(sbf.toString());
      }
      if (e.getActionCommand().compareTo("Paste")==0)
      {
         pasteDataIntoJTable("\t");
      }
      else if (e.getActionCommand().compareTo("PasteCSV")==0){
    	pasteDataIntoJTable(",");  
      }
   }
/**
 * 
 *<P>Once the imported content is set, this method can be called 
 *to take the imported content and put it into the respective JTable.</P>
 *<P><B>Limitations:</B>  </P>
 * @param dilimitedChar
 */
	private void pasteDataIntoJTable(string dilimitedChar){
		 //TODO: System.out.println("Trying to Paste");
         /*int startRow=(jTable1.getSelectedRows())[0];
         int startCol=(jTable1.getSelectedColumns())[0];*/
         int startRow=0;  //always start at the first row and column.
         int startCol=0;
         try
         {
            string trstring= this.importedContent;
            //TODO: System.out.println("string is:"+trstring);
           
             StringTokenizer st1=new StringTokenizer(trstring,"\n");

             double [] X = new double[st1.countTokens()];
             double [] Y = new double[st1.countTokens()];
             double min = 10000000.0;

double conv = 1.0;

if(FTableCalculatorConstants.importunits!=FTableCalculatorConstants.programunits)
{
    if(FTableCalculatorConstants.importunits==1)
        conv = 0.3048;
    else
        conv = 1/0.3048;

}

             for(int i=0;st1.hasMoreTokens();i++)
            {
               rowstring=st1.nextToken();
               string [] st2 = rowstring.split(dilimitedChar);
               X[i] = double.parseDouble(st2[0]);
               Y[i] = double.parseDouble(st2[1]);
              min = Math.min(Y[i], min);

            }
        for(int i = 0;i<Y.length;i++)

        {
            if(FTableCalculatorConstants.choice ==1)
             Y[i] = Y[i]-min;
            Y[i] = Y[i]*conv;
            X[i] = X[i]*conv;

             }

             for(int i=0;i<X.length;i++)
             {
                 jTable1.getModel().setValueAt(X[i].toString(), i, 0);
               jTable1.getModel().setValueAt(Y[i].toString(), i, 1);
             }

               // StringTokenizer st2=new StringTokenizer(rowstring,dilimitedChar);
               
        }catch(Exception ex){ex.printStackTrace();}
	}
	public void setImportedContent(string content){
		this.importedContent = content;
	}
}


