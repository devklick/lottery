import { Flex, Skeleton } from "@mantine/core";
import { CSSProperties, PropsWithChildren } from "react";

import { useWaitFor } from "../../common/hooks/time.hooks";
import FieldLabel from "../FieldLabel";

interface FieldWrapperProps {
  name?: string;
  description?: string;
  required?: boolean;
  loading?: boolean;
  minLoadingTime?: number;
  direction?: CSSProperties["flexDirection"];
}

export default function FieldWrapper({
  name,
  description,
  children,
  required,
  loading,
  minLoadingTime = 400,
  direction = "column",
}: PropsWithChildren<FieldWrapperProps>) {
  const showSkeleton = useWaitFor(loading ?? false, minLoadingTime);

  const skeleton = (
    <Skeleton visible={showSkeleton} flex="0 1 0">
      {children}
    </Skeleton>
  );

  if (!name) return skeleton;

  return (
    <Flex gap={"xs"} direction={direction} w="100%" className="field-wrapper">
      <FieldLabel
        name={name}
        required={required}
        description={description}
        loading={showSkeleton}
      />
      {skeleton}
    </Flex>
  );
}
