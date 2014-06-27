/**
 * <P> </P>
 * <P><B>Created:</B> Apr 5, 2007 </P>
 * <P><B>Revised:</B>   </P>
 * <P><B>Limitations:</B>  </P>
 * <P><B>Dependencies:</B>  </P>
 * <P><B>References:</B>   </P>

 * @author CSC for EPA/ORD/NERL
 *
 */
public class FtableAdapter : ExcelAdapter {

	/**
	 * @param myJTable
	 */
	public FtableAdapter(atcControls.atcGridSource myJTable, string calculatorType) {
		base(myJTable,calculatorType);
		
	}
	//when the user copies to clip board, select the entire table.
	//then pull the data off of the grid and create a string object
	//that is the formatted matrix that the UCI file expects.
	public void actionPerformed(ActionEvent e){
		 if (e.getActionCommand().compareTo("Copy")==0)
	      {
	         StringBuffer sbf=new StringBuffer();
	         // Check to ensure we have selected only a contiguous block of cells
	         int numcols= jTable1.getColumnCount();//jTable1.getSelectedColumnCount();
	         int numrows=jTable1.getRowCount();//jTable1.getSelectedRowCount();
	         
	         jTable1.selectAll();
	         
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
	         //write the name of the calculator to the string buffer
	       //xyang  sbf.append(" *** " + this.calculatorType + " ***\n");
                 
                 if (this.calculatorType == "TRAPEZOIDAL") {
                      string txt = plotdata.trapPanel.lblChannelDepth.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      double outvalue = plotdata.value[0];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.trapPanel.lblTopChannelWidth.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[1];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.trapPanel.lblSideChannelSlope.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[2];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.trapPanel.lblChannelLength.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[3];
                      sbf.append(outvalue + "***\n");
                      
                      /*txt = plotdata.trapPanel.lblChannelMannigsValue.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[4];
                      sbf.append(outvalue + "***\n");*/
                      
                     /* txt = plotdata.trapPanel.lblChannelAvgSlope.getText();
                      sbf.append(" *** " + txt + "      ");
                     outvalue = plotdata.value[5];
                      sbf.append(outvalue + "***\n");*/
                      
                      txt = plotdata.trapPanel.lblIncrement.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[6];
                      sbf.append(outvalue + "***\n");
                  }
                  else if(this.calculatorType == "PARABOLIC"){
                      
                     string txt = plotdata.parabolicPanel.lblChannelLength.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      double outvalue = plotdata.value[0];
                      sbf.append(outvalue + "***\n");    
                      
                      txt = plotdata.parabolicPanel.lblChannelWidth.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[1];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.parabolicPanel.lblChannelDepth.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[2];
                      sbf.append(outvalue + "***\n");
                      
                      /*txt = plotdata.parabolicPanel.lblChannelMannigsValue.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[3];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.parabolicPanel.lblChannelAvgSlope.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[4];
                      sbf.append(outvalue + "***\n");*/
                      
                      txt = plotdata.parabolicPanel.lblIncrement.getText();
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[5];
                      sbf.append(outvalue + "***\n");
                      
                      
                  }   
                  else if(this.calculatorType == "CIRCULAR"){
                      
                     string txt = plotdata.circPanel.lblChannelLength.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      double outvalue = plotdata.value[0];
                      sbf.append(outvalue + "***\n");  
                      
                      txt = plotdata.circPanel.lblChannelDiameter.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[1];
                      sbf.append(outvalue + "***\n");
                     
                      /*txt = plotdata.circPanel.lblChannelMannigsValue.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[2];
                      sbf.append(outvalue + "***\n");*/
                      
                      txt = plotdata.circPanel.lblChannelAvgSlope.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[3];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.circPanel.lblIncrement.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[4];
                      sbf.append(outvalue + "***\n");
                      
                  }   
                  else if(this.calculatorType == "RECTANGULAR"){
                      
                     string txt = plotdata.recPanel.lblChannelDepth.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      double outvalue = plotdata.value[0];
                      sbf.append(outvalue + "***\n");  
                      
                      txt = plotdata.recPanel.lblChannelWidth.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[1];
                      sbf.append(outvalue + "***\n");
                     
                      txt = plotdata.recPanel.lblChannelLength.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[2];
                      sbf.append(outvalue + "***\n");
                      
                      /*txt = plotdata.recPanel.lblChannelMannigsValue.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[3];
                      sbf.append(outvalue + "***\n");*/
                      
                      /*txt = plotdata.recPanel.lblChannelAvgSlope.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[4];
                      sbf.append(outvalue + "***\n");*/
                      
                      txt = plotdata.recPanel.lblIncrement.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[5];
                      sbf.append(outvalue + "***\n");
                  }   
                  
                  else if (this.calculatorType == "TRIANGULAR"){
                      
                     string txt = plotdata.triPanel.lblChannelDepth.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      double outvalue = plotdata.value[0];
                      sbf.append(outvalue + "***\n");  
                      
                      txt = plotdata.triPanel.lblChannelWidth.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[1];
                      sbf.append(outvalue + "***\n");
                     
                      txt = plotdata.triPanel.lblChannelLength.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[2];
                      sbf.append(outvalue + "***\n");
                      
                      /*txt = plotdata.triPanel.lblChannelMannigsValue.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[3];
                      sbf.append(outvalue + "***\n");
                      
                      txt = plotdata.triPanel.lblChannelAvgSlope.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[4];
                      sbf.append(outvalue + "***\n");*/
                      
                      txt = plotdata.triPanel.lblIncrement.getText();//xyang
                      sbf.append(" *** " + txt + "      ");
                      outvalue = plotdata.value[5];
                      sbf.append(outvalue + "***\n");
                  }   
                  //xyang end            
                 
	         sbf.append("  FTABLE    999\n");
	         //sbf.append("  999's are placeholders for user provided ID 1-3 digits in length***");
                 //sbf.append("\n");

                 sbf.append(" rows cols                               ***\n");
	         //the following section requires me to do a calculation to correctly right justify the rows and cols figures. 

	         sbf.append(FiveSpaceFormat(numrows.toString()) + FiveSpaceFormat(numcols.toString())+"\n");
	         for(int i= 0; i < numcols;i++){
	        	 string colName = jTable1.getColumnModel().getColumn(i).getHeaderValue().toString();
	        	 colName = colName.substring(0, colName.indexOf("("));
	        	 sbf.append(TenSpaceFormat(colName));
	         }
	         sbf.append(" ***");
	        sbf.append("\n");
	         for (int i=0;i<numrows;i++)
	         {
	            for (int j=0;j<numcols;j++)
	            {
	            	//jTable1.getValueAt(rowsselected[i],colsselected[j]);
	            	double newValue =double.valueOf(jTable1.getValueAt(rowsselected[i],colsselected[j]).toString());
	            	if (newValue.equals(double.NaN)){
	            		newValue = 0.000000d;
	            	}
	            	DecimalFormat df = new DecimalFormat("#.######");
//	            	if (newValue <= 9E-6){
//	            		sbf.append(TenSpaceFormat("0.00000"));
//	            	}
//	            	else{
	            		sbf.append(TenSpaceFormat(df.format(newValue)));	
	            	//}
	            	
	               //if (j<numcols-1) sbf.append("\t");
	            }
	            sbf.append("\n");
	         }
	         sbf.append("\n");
	         sbf.append("\n");
	         sbf.append(FiveSpaceFormat("  END FTABLE999"));
                
                 /*stsel  = new StringSelection(sbf.toString());
	         system = Toolkit.getDefaultToolkit().getSystemClipboard();
	         system.setContents(stsel,stsel);*/
	         this.jTArea.setText(sbf.toString());
	      }
	}
	private string NSpaceFormat(string content, int maxNumSpaces){
		int sl = content.length(); //sl = string length
		int startingpoint = maxNumSpaces-sl;
		string retValue = "";
		for (int i=0;i<startingpoint;i++){
			retValue += " ";
		}
		retValue += content;
		return retValue;
	}
	private string FiveSpaceFormat(string content){
		//take the length of the string
		int maxNumSpaces = 5;
		return NSpaceFormat(content, maxNumSpaces);
		
	}
	private string TenSpaceFormat(string content){
		//take the length of the string
		int maxNumSpaces = 10;
		return NSpaceFormat(content, maxNumSpaces);
		
	}
}
