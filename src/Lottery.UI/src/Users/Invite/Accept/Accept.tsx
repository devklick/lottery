import {
  Box,
  Button,
  Container,
  InputError,
  LoadingOverlay,
  Paper,
  PasswordInput,
  Stack,
  TextInput,
  Title,
} from "@mantine/core";
import { useForm, schemaResolver } from "@mantine/form";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import {
  AcceptInviteRequestBody,
  AcceptInviteResponse,
  acceptInviteRequestBodySchema,
  verifyInviteRequestQuerySchema,
} from "./accept.schema";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";
import userService from "../../userService";

type AcceptProps = object;

// eslint-disable-next-line no-empty-pattern
function Accept({}: AcceptProps) {
  const navigate = useNavigate();

  const queryValidation = useValidatedQueryParams(
    verifyInviteRequestQuerySchema,
  );

  const verifyInviteQuery = useQuery({
    queryKey: ["user/invite/accept"],
    enabled: false,
    refetchOnWindowFocus: false,
    refetchInterval: false,
    refetchOnMount: false,
    refetchOnReconnect: false,
    queryFn: () =>
      queryValidation.success
        ? userService.verifyInvite(queryValidation.data)
        : undefined,
  });

  const acceptInviteMutation = useMutation<
    AcceptInviteResponse,
    Array<{ message: string }>,
    AcceptInviteRequestBody
  >({
    mutationFn: (body) => userService.acceptInvite({ body }),
  });

  const form = useForm<AcceptInviteRequestBody>({
    validate: schemaResolver(acceptInviteRequestBodySchema),
    validateInputOnChange: true,
    initialValues: {
      email: queryValidation.success ? queryValidation.data.email : "",
      token: queryValidation.success ? queryValidation.data.token : "",
      confirmPassword: "",
      password: "",
      username: "",
    },
  });

  useEffect(() => {
    // If the params are valid, we need to verify that the invite is valid
    if (queryValidation.success) {
      verifyInviteQuery.refetch();
    } else {
      // otherwise we'll need to do something else
    }
  }, [queryValidation, verifyInviteQuery]);

  // If the mutation was successful, the new user account has successfully been created,
  // so we can redirect the user to the login screen
  useEffect(() => {
    if (acceptInviteMutation.isSuccess) {
      navigate("/account/signIn");
    }
  }, [acceptInviteMutation, navigate]);

  return (
    <Container p={0} maw={300}>
      <Box pos="relative">
        <LoadingOverlay
          visible={verifyInviteQuery.status == "pending"}
          zIndex={1000}
          overlayProps={{ radius: "sm", blur: 2 }}
        />
      </Box>
      <Title>Accept Invite</Title>
      <Paper shadow="xl" p={24} radius={10}>
        <form
          onSubmit={form.onSubmit((data) => acceptInviteMutation.mutate(data))}
        >
          <Stack gap={24}>
            <TextInput
              placeholder="Email"
              {...form.getInputProps("email")}
              value={queryValidation.success ? queryValidation.data.email : ""}
              disabled={true}
            />
            <TextInput
              placeholder="Username"
              {...form.getInputProps("username")}
            />

            <PasswordInput
              placeholder="Password"
              {...form.getInputProps("password")}
            />

            <PasswordInput
              placeholder="Confirm Password"
              {...form.getInputProps("confirmPassword")}
            />

            <Button variant="filled" type="submit">
              Submit
            </Button>

            {acceptInviteMutation.isError && (
              <InputError>
                {acceptInviteMutation.error.map((e) => e.message).join(". ")}
              </InputError>
            )}
          </Stack>
        </form>
      </Paper>
    </Container>
  );
}

export default Accept;
