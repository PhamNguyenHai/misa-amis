import { jwtDecode } from "jwt-decode";

export const decodeJwtToken = (accessToken) => {
  try {
    const decodedToken = jwtDecode(accessToken);
    return decodedToken;
  } catch (error) {
    console.error(error);
  }
};
