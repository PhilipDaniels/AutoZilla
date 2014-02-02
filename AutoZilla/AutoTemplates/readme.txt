AutoTemplates
=============
Drop a template file (*.azt) into this folder to have it automatically bound
to a HotKey. AutoZilla will paste the text when the HotKey is pressed. You may
use variables in the template, but you cannot move the cursor around or do any
other sophisticated processing - it is just "paste into current application".

The format of a template is as follows. There are two parts, with the marker
";;AZ;;" used to separate them. Before the marker is the AutoZilla
configuration section. You will need to specify a Key and a Name, but the
description is optional. Everything after the marker is the body of the
template.


[Config]
Key: CA-D
Name: Short name here
Description: Longer description here
  
;;AZ;; 
Body of the template goes here and can continue onto many lines
and use variables like ${DOMAINUSER}.
