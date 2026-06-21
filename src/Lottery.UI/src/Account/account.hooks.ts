import { useMutation, useQuery } from "@tanstack/react-query";

import accountService from "./accountService";

interface UseGetAccountProps {
  enabled: boolean;
}

export function useGetAccount({ enabled }: UseGetAccountProps) {
  return useQuery({
    queryKey: ["account", "get"],
    queryFn: () => accountService.getAccount(),
    enabled,
  });
}

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface UseUpdateAccountProps {}

export function useUpdateAccount(_props?: UseUpdateAccountProps) {
  return useMutation({
    mutationFn: accountService.updateAccount,
  });
}
