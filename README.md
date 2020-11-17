# Recruit

There are 3 tasks below to complete in 5 days. You can use  utility classes and source in the project.
The development expected shoul be in Scenes/MainScene. 3rth party Assets and resources can be used.

1- Menu: Create a scroll menu on the right side of the screen. Populate ui buttons per products  by calling a service that its informations are given in service-informations.txt file. You can call the service with user and password information given by e-mail. It should look like task1.png.
Note: Visual image of buttons shoul be populated by according response[x].products[x].thumbnailFileUrl

2- Event: Populate an event  on the action of the buttons pressed. 
  The Event should:
  a- Download the product from response[x].products[x].fileUrl as a zip file.
  b- Show the percentage of the downloading in the progress. It should look like task2.png.
  c- Exract the zip file. 
  d- Read and Import obj file
Note: You can use OBJLoader class to import obj files in the project.
 
 
3- Move: The Imported obj file shoul be interactable. 
Select, Move And Rotate movements by mouse actions should be available.
  

    
