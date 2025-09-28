import { toast } from "helpers/toast";

const createRequest = async (path: string, method: string, data: any = null) => {
  const companyId = JSON.parse(localStorage.getItem('companyId') || `"empty"`);

  const response = await fetch(path, {
    method: method,
    body: data ? JSON.stringify(data) : null,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem("token")}`,
      'X-Company': `${companyId}`
    }
  });

  if (response.status == 200 && response.headers.get("Content-type") == "text/html") {
    return await response.text();
  }

  if (response.status == 200) {
    return await response.json();
  }

  if (response.status == 400) {
    Object.values((await response.json()).errors).flat().map((e:any) => toast(e, { type: 'warning' }));
  }

  if ([401, 403].includes(response.status)) {
    location.href = '/login'
  }

  if ([404, 500].includes(response.status)) {
    throw { code: response.statusText.replaceAll(' ', ''), message: await response.text() }
  }
      
  return null
}

const get = (path: string) => createRequest(path, 'GET', null);
const post = (path: string, data: any) => createRequest(path, 'POST', data);
const put = (path: string, data: any) => createRequest(path, 'PUT', data);
const del = (path: string) => createRequest(path, 'DELETE');

export const http = { get, post, put, del }
