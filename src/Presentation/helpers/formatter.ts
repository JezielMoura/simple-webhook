export type DateOptions = {
  dateOnly?: boolean
}

const date = (value: any, { dateOnly = false }: DateOptions = {}) => {

  if (value == null || value == undefined)
    return null;

  const date = new Date(value);
  
  if (dateOnly)
    return date.toLocaleDateString("pt-BR", { timeZone: "UTC" });

  const dateValue = date.toLocaleDateString("pt-BR");
  const hoursValue = date.getHours().toString().padStart(2, '0');
  const minutesValue = date.getMinutes().toString().padStart(2, '0');
  
  return `${dateValue} ${hoursValue}:${minutesValue}`;
}

const currency = (value: any) => {
  const BRL = new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
  });

  return BRL.format(value ?? '0');
}
  
export const format = { date, currency }
  