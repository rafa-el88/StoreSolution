export class Movie {
  constructor(
    public title = '',
    public description = '',
    public sinopse = '',
    public pricePerDay = '',
    public quantityCopies = '',
    public unitsInStock = '',
    public isActive = ''
  ) { }
  public id = '';
  public moviesCount = 0;
}
