import { useMantineTheme } from "@mantine/core";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useConfirmEmailChange } from "./hooks";
import { confirmEmailChangePageQuerySchema } from "./schema";
import { notifyError, notifySuccess } from "../../../common/notifications";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";

export default function ConfirmEmailChange() {
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
      notifySuccess({
        title: "Email address updated",
        message: "Your email address has successfully been updated",
      });
    } else {
      notifyError({
        title: "Error updating email address",
        message: "We ran into a problem while updating your email address",
      });
    }
  }, [colors.green, colors.red, query.status]);

  return null;
}
