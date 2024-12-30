export class Customer {
  constructor(
    public name = '',
    public email = '',
    public phoneNumber = '',
    public address = '',
    public city = '',
    public gender = ''
  ) { }
  public id = '';
  public customerCount = 0;
}
