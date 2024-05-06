# Smart Home Box by Hamza

<h4>Project Description</h4>

The "Smart Home Box by Hamza" project represents a system for monitoring and controlling the home via the internet (IoT). On the executive side, there are two NodeMCU ESP8266 microcontrollers with WiFi modules. These microcontrollers are connected to various sensors and actuators for environmental monitoring and security in the space.

<h4>Components Used</h4>

Sensors:

- DHT11 for measuring air humidity and temperature
- PIR sensor for motion detection in the space
- Fire sensor for detecting fire using infrared rays
- MQ2 gas sensor for measuring the amount of natural gas in the room

Actuators:

- 2 passive buzzers
- 3W LED module
- Button sensor

<h4>Control Side</h4>

The control side of the system consists of a web application implemented in Angular with Bootstrap. Additionally, there is a web API implemented in ASP.NET Core (C#), connected to a SQL database.

The SQL database is used to store sensor detection history and for the Twilio service for sending notifications via WhatsApp and SMS, as well as the SMTP service for sending email notifications. Besides the SQL database, a real-time database is used on Firebase for storing real-time sensor data.

<h4>Application Functionalities</h4>

The application consists of the following sections:

- Gas/Fire: Display the current value of gas and fire detection, with the ability to turn off the alarm via the application or via the button sensor.
- Security System: Monitor motion detection in the space and only enable alarm deactivation.
- Temperature/Humidity: Display the current temperature and humidity in the space.
- Light: Enable manual control of the 3W LED light or set automatic activation within a specified time period.
- Profile: Allow users to change personal information and passwords.

<h4>Other Functionalities</h4>

- Alarm activation history and space temperature and humidity history.
- Possibility of multiplexing projects with the same code by entering access data.
- Notifications via email, SMS, or WhatsApp about detected dangers.
- Ability to log out all devices with current logins and retrieve a temporary password via email in case of a forgotten password.
- View current IP addresses from which the login to the application was made.

<h4>Executive Side Appearance</h4>

For a more aesthetically pleasing appearance, NodeMCU microcontrollers and sensors are housed in a branded box, with sensors attached to the outside.

This project provides a comprehensive home monitoring system that allows users to monitor and control various aspects of the environment and security of the space through a simple and elegant web application.

<h4>License</h4>

This project is licensed under the MIT License.