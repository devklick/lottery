import { useMantineTheme } from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck, IconXboxX } from "@tabler/icons-react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useConfirmEmailChange } from "./hooks";
import { confirmEmailChangePageQuerySchema } from "./schema";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface ConfirmEmailChangeProps {}

// eslint-disable-next-line no-empty-pattern
export default function ConfirmEmailChange({}: ConfirmEmailChangeProps) {
  const paramValidation = useValidatedQueryParams(
    confirmEmailChangePageQuerySchema,
  );
  const navigate = useNavigate();
  const { colors } = useMantineTheme();

  useEffect(() => {
    if (!paramValidation.success) {
      navigate("/home");
    }
  }, [navigate, paramValidation.success]);

  const query = useConfirmEmailChange(paramValidation.data);

  useEffect(() => {
    if (query.status === "pending") return;
    else if (query.status === "success") {
      notifications.show({
        title: "Email address updated",
        message: "Your email address has successfully been updated",
        icon: <IconCircleCheck />,
        color: colors.green[5],
      });
    } else {
      notifications.show({
        title: "Error updating email address",
        message: "We ran into a problem while updating your email address",
        icon: <IconXboxX />,
        color: colors.red[5],
      });
    }
  }, [colors.green, colors.red, query.status]);

  return null;
}
