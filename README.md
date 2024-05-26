# Smart home box by Hamza
<h3>Project Description</h3>
The "Smart Home Box by Hamza" project represents a system for monitoring and controlling the home via the internet (IoT). On the executive side, there are two NodeMCU ESP8266 microcontrollers with WiFi modules. These microcontrollers are connected to various sensors and actuators for environmental monitoring and security in the space. On the control side, a web application is developed using Angular with Bootstrap for the frontend, while the backend is powered by a web API implemented in ASP.NET Core (C#) connected to a SQL database. Additionally, the system utilizes Firebase real-time database for storing real-time sensor data.

<h3>Components Used</h3>
Sensors:

- DHT11 for measuring air humidity and temperature
- PIR sensor for motion detection in the space
- Fire sensor for detecting fire using infrared rays
- MQ2 gas sensor for measuring the amount of natural gas in the room
- Photoresistor sensor for determining ambient light levels and enabling Light/Night mode on the web page

Actuators:

- 2 passive buzzers
- 3W LED module
- Button sensor
<h3>Control Side</h3>
The control side of the system consists of a web application implemented in Angular with Bootstrap. Additionally, there is a web API implemented in ASP.NET Core (C#), connected to a SQL database.


The SQL database is used to store sensor detection history and for the Twilio service for sending notifications via WhatsApp and SMS, as well as the SMTP service for sending email notifications. Besides the SQL database, a real-time database is used on Firebase for storing real-time sensor data.

<h3>Application Functionalities</h3>
The application consists of the following sections:

- Gas/Fire: Display the current value of gas and fire detection, with the ability to turn off the alarm via the application or via the button sensor.
- Security System: Monitor motion detection in the space and only enable alarm deactivation.
- Temperature/Humidity: Display the current temperature and humidity in the space.
- Light: Enable manual control of the 3W LED light or set automatic activation within a specified time period.
- Profile: Allow users to change personal information and passwords.


Responsiveness: The application is responsible for managing all devices and is designed to be responsive, ensuring seamless control and monitoring across all screen sizes."
<h3>Other Functionalities</h3>

- Alarm activation history and space temperature and humidity history, including the ability to view this history on a line graph.
- Possibility of multiplexing projects with the same code by entering access data.
- Notifications via email, SMS, or WhatsApp about detected dangers.
- Ability to log out all devices with current logins and retrieve a temporary password via email in case of a forgotten password.
- View current IP addresses from which the login to the application was made and the ability to log out a specific device.
<h3> Testing the Application</h3>
The application is available for testing at the following URL: https://hamzabikic.github.io


You can sign in using the following credentials:


- Username: admin
- Password: adminadmin
<h3>Executive Side Appearance</h3>
For a more aesthetically pleasing appearance, NodeMCU microcontrollers and sensors are housed in a branded box, with sensors attached to the outside.


This project provides a comprehensive home monitoring system that allows users to monitor and control various aspects of the environment and security of the space through a simple and elegant web application.

<h3>Images</h3>
<h4>Executive side</h4>
<img src="/Images/vanjski1.jpg">
<img src="/Images/vanjski2.jpeg">
<img src="/Images/vanjski3.jpeg">
<img src="/Images/vanjski4.jpeg">
<img src="/Images/vanjski5.jpeg">
<h4>Control side - computer</h4>
<img src="/Images/login.JPG">
<img src="/Images/password-change.JPG">
<img src="/Images/profile.JPG">
<img src="/Images/temphumchart.JPG">
<img src="/Images/temphumhistory.JPG">
<img src="/Images/gas-fire.JPG">
<img src="/Images/security.JPG">
<img src="/Images/light.JPG">
<img src="/Images/ipaddresses.JPG">
<h4>Control side - smartphone</h4>
<img style="width:400px" src="/Images/mob_login.jpeg">
<img style="width:400px" src="/Images/mob_password.jpeg">
<img style="width:400px" src="/Images/temphumchart-mob.jpg">
<img style="width:400px" src="/Images/temphumhistory-mob.jpg">
<img style="width:400px" src="/Images/mob_fire.jpeg">
<img style="width:400px" src="/Images/mob_security.jpeg">
<img style="width:400px" src="/Images/mob_light.jpeg">
<img style="width:400px" src="/Images/mob_profile.jpeg">
<img style="width:400px" src="/Images/mob_ipaddresses.jpeg">
<h4>Notifications</h4>
<h5>SMS notifications</h5>
<img style="width:400px" src="/Images/notifikacija-sms.jpg">
<h5>WhatsApp notifications</h5>
<img style="width:400px" src="/Images/notifikacija-wp.jpg">
<h5>Email notifications</h5>
<img style="width:400px" src="/Images/notifikacija-email.jpg">


<h3>License</h3>
This project is protected by the "Proprietary No-Use License". See the LICENSE file for more information.
