import { Skeleton, Stack } from "@mantine/core";
import { PropsWithChildren } from "react";

import { useWaitFor } from "../../common/hooks/time.hooks";
import FieldLabel from "../FieldLabel";

interface FieldWrapperProps {
  name?: string;
  description?: string;
  required?: boolean;
  loading?: boolean;
  minLoadingTime?: number;
}

export default function FieldWrapper({
  name,
  description,
  children,
  required,
  loading,
  minLoadingTime = 400,
}: PropsWithChildren<FieldWrapperProps>) {
  const showSkeleton = useWaitFor(loading ?? false, minLoadingTime);

  const skeleton = <Skeleton visible={showSkeleton}>{children}</Skeleton>;

  if (!name) return skeleton;

  return (
    <Stack gap={"xs"}>
      <FieldLabel
        name={name}
        required={required}
        description={description}
        loading={showSkeleton}
      />
      {skeleton}
    </Stack>
  );
}
