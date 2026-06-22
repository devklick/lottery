import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";
import { z } from "zod";

function useValidatedQueryParams<T extends z.ZodRawShape>(
  schema: z.ZodObject<T>,
) {
  const [rawParams] = useSearchParams();

  return useMemo(
    () => schema.safeParse(Object.fromEntries(rawParams.entries())),
    [rawParams, schema],
  );
}

export default useValidatedQueryParams;
