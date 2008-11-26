Clean sediment utility developed November 2008 by :

Lloyd Chris Wilson, Ph.D., P.E. 
Clayton Engineering
11920 Westline Industrial Dr.
St. Louis, MO 63146
314-692-8888
cwilson@claytoneng.pro

Developed under contract to Jack Kittle at AquaTerra, Inc., for Tim Wool EPA Region IV.
Clayton Engineering Project 07162.1

Status as of November 26, 2008:

This tool was designed to mimic features of WCS Sediment Tool formerly developed by TetraTech for ArcView. The code was completely rewritten from the original Avenue script and contains both a subset and superset of WCS features:

Superset: 

- all calculations done on grid basis (WCS used average values of LS)

Subset:

- only one of the four sediment delivery ratio methods is implemented; unable to obtain original papers for all methods, and WCS tool would no longer run to do comparisons. WCS made calls to Sediment.dll for some calcs and source code for that was not available. Finally, hand-calculations using equations reported in WCS yielded non-sensical results.

- road erosion component not included: unable to acquire original methodology or papers for this poorly documented routine; unsure whether GIS layers exist with required information

Additional future work suggested:

- add channel erosion component