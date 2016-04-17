int flexSensorPin = A1; //analog pin 0
int fsrPin = 0;     // the FSR and 10K pulldown are connected to a0

void setup(){
  Serial.begin(9600);
}

void loop(){
  int flexSensorReading = analogRead(flexSensorPin);
  int flexValue = map(flexSensorReading, 200, 500, 0, 300);
  Serial.print(flexSensorReading);

  Serial.print(" ");
  
  int fsrValue = analogRead(fsrPin);
  Serial.println(fsrValue);     // the raw analog reading

  delay(250); 
}

 
