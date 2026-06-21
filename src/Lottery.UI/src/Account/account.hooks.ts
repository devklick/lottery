import { useQuery } from "@tanstack/react-query";

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
