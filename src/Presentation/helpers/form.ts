const parse = (form: FormData) => {
  let output: any = {};

  form.forEach((value, key) => {
    if (key.includes('.')) {
      const [object, index, field] = key.split('.')

      if(!output[object]) {
        output[object] = [];
      }

      if (!(output[object][index])) {
        output[object][index] = {}
      }

      output[object][index][field] = value
      return;
    }

    if(!Reflect.has(output, key)) {
      output[key] = value;
      return;
    }

    if(!Array.isArray(output[key])) {
      output[key] = [output[key]];
    }
    
    output[key].push(value);
  });

  return output;
}

export const form =  { parse }