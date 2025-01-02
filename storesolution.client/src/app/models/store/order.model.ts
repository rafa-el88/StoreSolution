export class Order {
  constructor(
    public DateStartRental = '',
    public DateEndRental = '',
    public DateDevolution = '',
    public ReturnedMovie = '',
  ) { }
  public id = '';
  public ordersCount = 0;
}
