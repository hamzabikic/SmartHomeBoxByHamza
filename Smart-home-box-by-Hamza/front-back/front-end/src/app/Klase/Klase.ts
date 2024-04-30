export interface temphumlista {
  infoLista : TemperaturaVlaznost[]
}
export interface TemperaturaVlaznost {
  id:number,
  datum:Date,
  vrijeme:string,
  temperatura:number,
  vlaznost:number
}
export interface gasfirelista {
  lista:gasfire[];
}
export interface gasfire {
  id:number,
  datum:string,
  vrijeme:string,
  gasValue:number,
  fireDetected:boolean
}
export interface PokretiResponse {
  pokreti :Pokret[];
}
export interface Pokret {
  id:number,
  datum:string,
  vrijeme:string
}
export interface KorisnikInfo {
  ime:string,
  prezime:string,
  email:string,
  telefon:string,
  smsSlanje:boolean,
  whatsAppSlanje:boolean,
  emailSlanje : boolean
}
