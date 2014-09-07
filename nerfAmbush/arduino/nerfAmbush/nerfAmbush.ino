
const int triggerVal = 175;
const int rearmTime = 150*1000;
boolean armed = false;

void setup() {
  Serial.begin(9600);
  Serial.println("Powerd On");
  delay(2000);
  Serial.println("armed");
  armed = true;
}

int heightest = 0;

void loop() {
  int sens = analogRead(A0);
  if (sens > heightest) { 
    heightest = sens;  
    Serial.println("max: " + String(heightest));
  }
  
//  Serial.println(String(sens) + " > " + String(triggerVal));
  if (armed && sens > triggerVal) {
//    delay(100);
//    armed = false;
    Serial.println("triggered"); 
    delay(rearmTime);
    Serial.println("armed");
  }
}


