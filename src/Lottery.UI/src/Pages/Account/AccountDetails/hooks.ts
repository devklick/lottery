import { useMutation } from "@tanstack/react-query";

import accountService from "../accountService";

export function useChangePassword() {
  return useMutation({
    mutationFn: accountService.changePassword,
  });
}
